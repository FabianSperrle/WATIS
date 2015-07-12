using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;
using Microsoft.ComplexEventProcessing;


namespace ConsoleApplication1
{
    class IntervalCreation
    {
   /** Termlist. */
   private ISet<String> termList = new HashSet<String>();
   /** Intervallist. */
   private List<Interval> intervalList = new List<Interval>();
   /** List of tuples. */
   private  List<PointEvent<TwitterDataTerm>> tuples;
   /** Position of attribute. */
   private  int attributePosition;
   /** Distance TimePoints. */
   private  int distanceTimePoints;
   /** List of terms and tuples in which they are included. */
   private  Dictionary<String, ISet<PointEvent<TwitterDataTerm>>> termTuples = new Dictionary<String, ISet<PointEvent<TwitterDataTerm>>>();
   /** List of coocurrence terms for terms. */
   private  Dictionary<String, Dictionary<String, int>> coocTerms = new Dictionary<String, Dictionary<String, int>>();
   /** Flag for keeping tuples. */
   private  bool keepTuples;
   /** Flag for keeping coocurrence terms. */
   private  bool keepCoocs;

   /**
    * Default constructor.
    * 
    * @param tuples
    *           list of tuples
    * @param distanceTimePoints
    *           in seconds between an interval
    * @param attributePosition
    *           position of attribute
    * @param keepTuples
    *           flag for keeping tuples
    * @param keepCoocs
    *           flag for keeping cooccurrence terms
    */
   public IntervalCreation( List<PointEvent<TwitterDataTerm>> tuples,  int distanceTimePoints,
          int attributePosition,  bool keepTuples,  bool keepCoocs) {
      this.tuples = tuples;
      this.attributePosition = attributePosition;
      this.distanceTimePoints = distanceTimePoints;
      this.keepTuples = keepTuples;
      this.keepCoocs = keepCoocs;
      this.createIntervals();
   }

   /**
    * Returns intervallist.
    * 
    * @return list of intervals
    */
   public List<Interval> getIntervalList() {
      return this.intervalList;
   }

   /**
    * Returns termlist.
    * 
    * @return list of terms
    */
   public ISet<String> getTermList() {
      return this.termList;
   }

   /**
    * Returns count of tuples.
    * 
    * @return count of tuples
    */
   public int getTupleCount() {
      return this.tuples.Count;
   }
   
   /**
    * Returns ISet of tuples for term.
    * @param term term
    * @return ISet of tuples
    */
   public ISet<PointEvent<TwitterDataTerm>> getTuplesForTerm( String term) {
      return this.termTuples[term];
   }
   
   /**
    * Returns the total tuple count per term.
    * @param term term
    * @return count of tuples
    */
   public double getTupleCountForTerm( String term) {
      return this.getTermCountInIntervals(term).Sum();
   }

   /**
    * Get the count of a term in each interval.
    * 
    * @param term
    *           the term to get the count from
    * @return int[] each index of the array represents the count of a term in interval 'index+1'
    */
   public double[] getTermCountInIntervals( String term) {
       double[] nwt = new double[intervalList.Count()];
      int i = 0;
      foreach (Interval interval in intervalList) {
         nwt[i] = interval.getCountByTerm(term);
         i++;
      }
      return nwt;
   }

   /**
    * Get the count of all tuples in each interval.
    * 
    * @return int[] each index of the array represents the count of all tuples in interval 'index+1'
    */
   public double[] getTupleCountInIntervals() {
       double[] nt = new double[this.intervalList.Count()];
      int i = 0;
      foreach (Interval interval in intervalList) {
         nt[i] = interval.getTupleCount();
         i++;
      }
      return nt;
   }

   /**
    * Creates intervals.
    */
   private void createIntervals() {
      long tupleTimestamp;
      long startTimestampInterval = 0;
      long endTimestampInterval = 0;
      // is used in createNewInterval()
      this.intervalList = new List<Interval>();
      Interval interval = null;

      foreach (PointEvent<TwitterDataTerm> tuple in this.tuples) {
         tupleTimestamp = tuple.StartTime.UtcTicks;
         // first interval
         if (startTimestampInterval == 0 && endTimestampInterval == 0) {
            startTimestampInterval = tupleTimestamp;
            endTimestampInterval = tupleTimestamp + (this.distanceTimePoints * 1000);
            interval = new Interval(this.attributePosition);
            this.intervalList.add(interval);
         }
         if (tupleTimestamp >= endTimestampInterval) {
            // here we have a new interval starting
            startTimestampInterval = tupleTimestamp;
            endTimestampInterval = tupleTimestamp + (this.distanceTimePoints * 1000);
            interval = new Interval(this.attributePosition);
            this.intervalList.add(interval);
         }
         interval.addTuple(tuple, this.termList, this.termTuples, this.keepTuples, this.coocTerms, this.keepCoocs);
      }
   }

   /**
    * InnerClass for an Interval (part of EDCOW).
    * 
    * @author Andreas Weiler &lt;andreas.weiler@uni.kn&gt;
    * @author Harry Schilling &lt;harry.schilling@uni.kn&gt;
    * @version 1.0
    */
   private  class Interval {

      /** List of tuples. */
      private List<PointEvent<TwitterDataTerm>> tuples;
      /** TermCounts. */
      private Dictionary<String, int> termCounts;
      /** AttributePosition. */
      private string attributeName;
      
      /**
       * Default constructor.
       * @param attributePosition
       *         position of attribute
       */
      //private Interval( int attributePosition) {
      private Interval(string  attributeName) {
         this.attributeName = attributeName;
         this.tuples = new List<PointEvent<TwitterDataTerm>>();
         this.termCounts = new Dictionary<String, int>();
      }
      
      /**
       * Adds a tuple to the list.
       * @param tuple tuple
       * @param termList list of terms
       * @param termTuples list of tuples per term
       * @param keepTuples keeps tuples in a list
       * @param coocs list of coocs per term
       * @param keepCoocs keeps coocs in a list
       */
      public void addTuple( PointEvent<TwitterDataTerm> tuple,  ISet<String> termList,
             Dictionary<String, ISet<PointEvent<TwitterDataTerm>>> termTuples,  bool keepTuples,
             Dictionary<String, Dictionary<String, int>> coocs,  bool keepCoocs) {
         this.tuples.Add(tuple);
         this.addTerms(tuple, termList, termTuples, keepTuples, coocs, keepCoocs);
      }
      
      /**
       * Returns tuple count.
       * @return count of tuples
       */
      public int getTupleCount() {
         return this.tuples.Count();
      }
  
      /**
       * Adds all terms of tuple to list.
       * @param tuple tuple
       * @param termList list of terms
       * @param termTuples list of tuples per term
       * @param keepTuples keeps tuples in a list
       * @param coocs list of coocs per term
       * @param keepCoocs keeps coocs in a list
       */
      private void addTerms( PointEvent<TwitterDataTerm> tuple,  ISet<String> termList,
             Dictionary<String, ISet<PointEvent<TwitterDataTerm>>> termTuples,  bool keepTuples,
             Dictionary<String, Dictionary<String, int>> coocs,  bool keepCoocs) {
          List<String> content = this.removeDuplicates(tuple.Payload.TwitterData.TWEET_CONTENT); // WROOOOOOOOOONG FIND_ME
         foreach (String str in content) {
            if (this.termCounts.ContainsKey(str)) {
                int value = this.termCounts[str];
               this.termCounts[str] = value + 1;
            } else {
               this.termCounts[str] = 1;
               termList.Add(str);
            }
            if (keepTuples) {
               this.addTuples(tuple, str, termTuples);
            }
            if (keepCoocs) {
               this.addCoocs(content, str, coocs);
            }
         }
      }
      
      /**
       * Adds tuples to term.
       * @param tuple tuple
       * @param str term
       * @param termTuples list of tuples per term
       */
      private void addTuples( PointEvent<TwitterDataTerm> tuple,  String str,  Dictionary<String, ISet<PointEvent<TwitterDataTerm>>> termTuples) {
         ISet<PointEvent<TwitterDataTerm>> tuples = termTuples.get(str);
         if (tuples == null) { tuples = new HashSet<PointEvent<TwitterDataTerm>>(); }
         tuples.add(tuple);
         termTuples.put(str, tuples);
      }
      
      /**
       * Adds coocs to term.
       * @param content content
       * @param str term
       * @param coocs list of coocs per term
       */
      private void addCoocs( List<String> content,  String str,
             Dictionary<String, Dictionary<String, int>> coocs) {
         Dictionary<String, int> strcoocs = coocs.get(str);
         if (strcoocs == null) { strcoocs = new Dictionary<String, int>(); }
         for (String s : content) {
            if (!s.equals(str)) {
               if (strcoocs.containsKey(s)) {
                   int tmp = strcoocs.get(s) + 1;
                  strcoocs.put(s, tmp);
               } else {
                  strcoocs.put(s, 1);
               }
            }
         }
         coocs.put(str, strcoocs);
      }
      
      /**
       * Removes duplicates.
       * @param list of terms
       * @return duplicate free list
       */
      private List<String> removeDuplicates( List<String> list) {
          HashSet<String> hs = new HashSet<String>();
         hs.addAll(list);
         list.clear();
         list.addAll(hs);
         return list;
      }
      
      /**
       * Get count by term.
       * @param term term
       * @return count of term
       */
      public int getCountByTerm( String term) {
         if (this.termCounts.containsKey(term)) { 
            return this.termCounts.get(term); 
         } else { 
            return 0;
         }
      }
   }
   
   /**
    * Sorts the Dictionary by value.
    * 
    * @param <K>
    *            the key
    * @param <V>
    *            the value
    * @param cache 
    *          Dictionary to sort
    * @param n 
    *          number of resultitems
    * @return sorted Dictionary (top n values)
    */
   private <K, V> Entry<K, V>[] sortDictionaryByValue( Dictionary<K, V> cache,  int n) {
       PriorityQueue<Entry<K, V>> hits = new PriorityQueue<Entry<K, V>>(n + 1, new 
            Comparator<Entry<K, V>>() {
         public int compare( Entry<K, V> arg0,  Entry<K, V> arg1) {
            return ((int) arg0.getValue()).compareTo((int) arg1.getValue()); 
         }
      });
      for (Entry<K, V> e : cache.entrySet()) {
         hits.add(e);
         if (hits.size() > n) {
            hits.remove();
         }
      }
      @SuppressWarnings("unchecked")
       Entry<K, V>[] result = new Entry[hits.size()];
      for (int i = result.length; --i >= 0;) {
         result[i] = hits.remove();
      }
      return result;
   }}}}

