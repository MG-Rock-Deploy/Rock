﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization;

using Ical.Net;
using Ical.Net.DataTypes;

using Rock;
using Rock.Data;
using Rock.Web.Cache;
using Rock.Lava;

namespace Rock.Model
{
    /// <summary>
    /// Represents a Scheduled event in Rock.  Several places where this has been used includes Check-in scheduling and Kiosk scheduling.
    /// </summary>
    [RockDomain( "Core" )]
    [Table( "Schedule" )]
    [DataContract]
    public partial class Schedule : Model<Schedule>, ICategorized, IHasActiveFlag, IOrdered, ICacheable
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the Name of the Schedule. This property is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> that represents the Name of the Schedule.
        /// </value>
        [MaxLength( 50 )]
        [DataMember]
        [IncludeForReporting]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a user defined Description of the Schedule.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the Description of the Schedule.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the content lines of the iCalendar
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/>representing the  content of the iCalendar.
        /// </value>
        [DataMember]
        public string iCalendarContent
        {
            get
            {
                return _iCalendarContent ?? string.Empty;
            }

            set
            {
                _getICalEvent = null;
                _iCalendarContent = value;
            }
        }

        private string _iCalendarContent;

        /// <summary>
        /// Gets or sets the number of minutes prior to the Schedule's start time  that Check-in should be active. 0 represents that Check-in 
        /// will not be available to the beginning of the event.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing how many minutes prior the Schedule's start time that Check-in should be active. 
        /// 0 means that Check-in will not be available to the Schedule's start time. This schedule will not be available if this value is <c>Null</c>.
        /// </value>
        [DataMember]
        public int? CheckInStartOffsetMinutes { get; set; }

        /// <summary>
        /// Gets or sets the number of minutes following schedule start that Check-in should be active. 0 represents that Check-in will only be available
        /// until the Schedule's start time.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing how many minutes following the Schedule's end time that Check-in should be active. 0 represents that Check-in
        /// will only be available until the Schedule's start time.
        /// </value>
        [DataMember]
        public int? CheckInEndOffsetMinutes { get; set; }

        /// <summary>
        /// Gets or sets the Date that the Schedule becomes effective/active. This property is inclusive, and the schedule will be inactive before this date. 
        /// </summary>
        /// <value>
        /// A <see cref="System.DateTime"/> value that represents the date that this Schedule becomes active.
        /// </value>
        [DataMember]
        [Column( TypeName = "Date" )]
        public DateTime? EffectiveStartDate { get; set; }

        /// <summary>
        /// Gets or sets that date that this Schedule expires and becomes inactive. This value is inclusive and the schedule will be inactive after this date.
        /// </summary>
        /// <value>
        /// A <see cref="System.DateTime"/> value that represents the date that this Schedule ends and becomes inactive.
        /// </value>
        [DataMember]
        [Column( TypeName = "Date" )]
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// Gets or sets the weekly day of week.
        /// </summary>
        /// <value>
        /// The weekly day of week.
        /// </value>
        [DataMember]
        public DayOfWeek? WeeklyDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the weekly time of day.
        /// </summary>
        /// <value>
        /// The weekly time of day.
        /// </value>
        [DataMember]
        public TimeSpan? WeeklyTimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets the CategoryId of the <see cref="Rock.Model.Category"/> that this Schedule belongs to.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the CategoryId of the <see cref="Rock.Model.Category"/> that this Schedule belongs to. This property will be null
        /// if the Schedule does not belong to a Category.
        /// </value>
        [DataMember]
        [IncludeForReporting]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [auto inactivate when complete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [auto inactivate when complete]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool AutoInactivateWhenComplete { get; set; } = false;

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets a value indicating whether Check-in is enabled for this Schedule.
        /// </summary>
        /// <value>
        /// A <see cref="System.Boolean"/> that is <c>true</c> if this instance is check in enabled; otherwise, <c>false</c>.
        /// <remarks>
        /// The <c>CheckInStartOffsetMinutes</c> is used to determine if Check-in is enabled. If the value is <c>null</c>, it is determined that Check-in is not 
        /// enabled for this Schedule.
        /// </remarks>
        /// </value>
        public virtual bool IsCheckInEnabled
        {
            get
            {
                return CheckInStartOffsetMinutes.HasValue && IsActive;
            }
        }

        #region Additional Lava Properties

        /// <summary>
        /// Gets the weekly time of day in friendly text, such as "7:00 PM".
        /// </summary>
        /// <value>
        /// The weekly time of day in friendly text or an empty string if not valid.
        /// </value>
        [LavaVisible]
        public string WeeklyTimeOfDayText => WeeklyTimeOfDay?.ToTimeString() ?? string.Empty;

        /*
            2021-02-17 - DJL

            These properties exist to simplify Lava code that needs to query if the schedule or check-in is currently active.
            They have been reinstated at the request of the community after being marked obsolete in v1.8.

            Reason: Community Request, Issue #3471 (https://github.com/SparkDevNetwork/Rock/issues/3471)
        */

        /// <summary>
        /// Gets a value indicating whether this schedule is currently active. This
        /// is based on <see cref="RockDateTime.Now" />. Use <see cref="Campus.CurrentDateTime"/> and <see cref="WasScheduleActive(DateTime)"/>
        /// to get this based on the Campus's current datetime. 
        /// </summary>
        /// <value>
        /// <c>true</c> if this schedule is currently active; otherwise, <c>false</c>.
        /// </value>
        [LavaVisible]
        public virtual bool IsScheduleActive
        {
            get
            {
                return WasScheduleActive( RockDateTime.Now );
            }
        }

        /// <summary>
        /// Gets a value indicating whether check-in is currently active for this schedule. This
        /// is based on <see cref="RockDateTime.Now" />. Use <see cref="Campus.CurrentDateTime"/> and <see cref="WasCheckInActive(DateTime)"/>
        /// to get this based on the Campus's current datetime. 
        /// </summary>
        /// <value>
        ///  A <see cref="System.Boolean"/> that is  <c>true</c> if Check-in is currently active for this Schedule ; otherwise, <c>false</c>.
        /// </value>
        [LavaVisible]
        public virtual bool IsCheckInActive
        {
            get
            {
                return WasCheckInActive( RockDateTime.Now );
            }
        }

        /// <summary>
        /// Gets the next start time based on <see cref="RockDateTime.Now" />. Use <see cref="Campus.CurrentDateTime"/>
        /// and <see cref="GetNextStartDateTime(DateTime)"/> to get this based on the Campus's current datetime. 
        /// </summary>
        /// <returns></returns>
        [LavaVisible]
        public virtual DateTime? NextStartDateTime
        {
            get
            {
                return GetNextStartDateTime( RockDateTime.Now );
            }
        }

        #endregion

        /// <summary>
        /// Gets the type of the schedule.
        /// </summary>
        /// <value>
        /// The type of the schedule.
        /// </value>
        public virtual ScheduleType ScheduleType
        {
            get
            {
                var calendarEvent = GetICalEvent();
                if ( calendarEvent != null && calendarEvent.DtStart != null )
                {
                    return !string.IsNullOrWhiteSpace( this.Name ) ?
                        ScheduleType.Named : ScheduleType.Custom;
                }
                else
                {
                    return WeeklyDayOfWeek.HasValue ?
                        ScheduleType.Weekly : ScheduleType.None;
                }
            }
        }

        /// <summary>
        /// Gets the next start date time.
        /// </summary>
        /// <param name="currentDateTime">The current date time.</param>
        /// <returns></returns>
        public DateTime? GetNextStartDateTime( DateTime currentDateTime )
        {
            if ( this.IsActive )
            {
                // Increase this from 1 to 2 years to catch events more than a year out. See github issue #4812.
                var endDate = currentDateTime.AddYears( 2 );

                var calEvent = GetICalEvent();

                Ical.Net.Interfaces.DataTypes.IRecurrencePattern rrule = null;

                if ( calEvent != null )
                {
                    if ( calEvent.RecurrenceRules.Any() )
                    {
                        rrule = calEvent.RecurrenceRules[0];
                    }
                }

                /* 2020-06-24 MP
                 * To improve performance, only go out a week (or so) if this is a weekly or daily schedule.
                 * If this optimization fails to find a next scheduled date, fall back to looking out a full two years
                 */

                if ( rrule?.Frequency == FrequencyType.Weekly )
                {
                    var everyXWeeks = rrule.Interval;
                    endDate = currentDateTime.AddDays( everyXWeeks * 7 );
                }
                else if ( rrule?.Frequency == FrequencyType.Daily )
                {
                    var everyXDays = rrule.Interval;
                    endDate = currentDateTime.AddDays( everyXDays );
                }

                var occurrences = GetScheduledStartTimes( currentDateTime, endDate );
                var nextOccurrence = occurrences.Min( o => ( DateTime? ) o );
                if ( nextOccurrence == null && endDate < currentDateTime.AddYears( 2 ) )
                {
                    // if tried an earlier end date, but didn't get a next datetime,
                    // use the regular way and see if there is a next schedule date within the next two years
                    endDate = currentDateTime.AddYears( 2 );
                    occurrences = GetScheduledStartTimes( currentDateTime, endDate );
                    nextOccurrence = occurrences.Min( o => ( DateTime? ) o );
                }

                return nextOccurrence;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the first start date time.
        /// </summary>
        /// <value>
        /// The first start date time.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual DateTime? FirstStartDateTime => GetFirstStartDateTime();

        /// <summary>
        /// Gets the first start date time this week.
        /// </summary>
        /// <value>
        /// The first start date time this week.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual DateTime? FirstStartDateTimeThisWeek
        {
            get
            {
                var endDate = RockDateTime.Today.SundayDate();
                var startDate = endDate.AddDays( -7 );
                var occurrences = GetScheduledStartTimes( startDate, endDate );
                return occurrences.Min( o => ( DateTime? ) o );
            }
        }

        /// <summary>
        /// Gets the start time of day.
        /// </summary>
        /// <value>
        /// The start time of day.
        /// </value>
        [LavaVisible]
        public virtual TimeSpan StartTimeOfDay
        {
            get
            {
                var calendarEvent = GetICalEvent();
                if ( calendarEvent != null && calendarEvent.DtStart != null )
                {
                    return calendarEvent.DtStart.Value.TimeOfDay;
                }

                if ( WeeklyTimeOfDay.HasValue )
                {
                    return WeeklyTimeOfDay.Value;
                }

                return new TimeSpan();
            }
        }

        /// <summary>
        /// Gets the duration in minutes.
        /// </summary>
        /// <value>
        /// The duration in minutes.
        /// </value>
        [LavaVisible]
        public virtual int DurationInMinutes
        {
            get
            {
                var calendarEvent = GetICalEvent();
                if ( calendarEvent != null && calendarEvent.DtStart != null && calendarEvent.DtEnd != null )
                {
                    return ( int ) calendarEvent.DtEnd.Subtract( calendarEvent.DtStart ).TotalMinutes;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Category"/> that this Schedule belongs to.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Category"/> that this Schedule belongs to.  If it does not belong to a <see cref="Rock.Model.Category"/> this value will be null.
        /// </value>
        [DataMember]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets the friendly schedule text.
        /// </summary>
        /// <value>
        /// The friendly schedule text.
        /// </value>
        [LavaVisible]
        [DataMember]
        public virtual string FriendlyScheduleText
        {
            get { return ToFriendlyScheduleText(); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if this is an active schedule. This value is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.Boolean"/> value that is <c>true</c> if this schedule is active, otherwise <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        [Previewable]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the order.
        /// Use <see cref="ExtensionMethods.OrderByOrderAndNextScheduledDateTime" >List&lt;Schedule&gt;().OrderByOrderAndNextScheduledDateTime</see>
        /// to get the schedules in the desired order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        [DataMember]
        public int Order { get; set; }

        #endregion

        #region Public Methods

        #region ICacheable

        /// <summary>
        /// Gets the cache object associated with this Entity
        /// </summary>
        /// <returns></returns>
        public IEntityCache GetCacheObject()
        {
            if ( this.Name.IsNotNullOrWhiteSpace() )
            {
                return NamedScheduleCache.Get( this.Id );
            }

            return null;
        }

        /// <summary>
        /// Updates any Cache Objects that are associated with this entity
        /// </summary>
        /// <param name="entityState">State of the entity.</param>
        /// <param name="dbContext">The database context.</param>
        public void UpdateCache( EntityState entityState, Rock.Data.DbContext dbContext )
        {
            NamedScheduleCache.FlushItem( this.Id );
        }

        #endregion ICacheable

        /// <summary>
        /// Pres the save changes.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="state">The state.</param>
        public override void PreSaveChanges( Data.DbContext dbContext, EntityState state )
        {
            EnsureEffectiveStartEndDates();

            base.PreSaveChanges( dbContext, state );
        }

        /// <summary>
        /// Ensures that the EffectiveStartDate and EffectiveEndDate are set correctly based on the CalendarContent.
        /// Returns true if any changes were made.
        /// </summary>
        /// <returns></returns>
        internal bool EnsureEffectiveStartEndDates()
        {
            /*
             * 12/6/2019 BJW
             *
             * There was an issue in this code because DateTime.MaxValue was being used to represent [no end date]. Because EffectiveEndDate
             * is a Date only in SQL, the time portion gets dropped.  Because 12/31/9999 11:59 != 12/31/9999 00:00, this caused false readings
             * when testing for schedules with no end dates. When there was no end date, Rock was calculating all occurrences with
             * GetOccurrences( DateTime.MinValue, DateTime.MaxValue ) which caused timeouts.  We should only call that GetOccurrences method
             * like that when we know there are a reasonable number of occurrences. 
             */

            var calEvent = GetICalEvent();
            if ( calEvent == null )
            {
                return false;
            }

            var originalEffectiveEndDate = EffectiveEndDate;
            var originalEffectiveStartDate = EffectiveStartDate;

            // Set initial values for start and end dates.
            // End date is set to MaxValue as a placeholder - infinitely repeating schedules should be stored with
            // a null end date value.
            DateTime? effectiveStartDateTime = calEvent.DtStart?.Value.Date;
            DateTime? effectiveEndDateTime = DateTime.MaxValue;

            // In Rock it is possible to set a rule with an end date, no end date or a number
            // of occurrences. The count property in the iCal rule refers to the count of occurrences.
            var endDateRules = calEvent.RecurrenceRules.Where( rule => rule.Count <= 0 );
            var countRules = calEvent.RecurrenceRules.Where( rule => rule.Count > 0 );

            var hasRuleWithEndDate = endDateRules.Any();
            var hasRuleWithCount = countRules.Any();
            var hasDates = calEvent.RecurrenceDates.Any();

            bool adjustEffectiveDateForLastOccurrence = false;

            // If there are any recurrence rules with no end date, the Effective End Date is infinity
            // iCal rule.Until will be min value date if it is representing no end date (backwards from Rock using max value)
            if ( hasRuleWithEndDate )
            {
                if ( endDateRules.Any( rule => RockDateTime.IsMinDate( rule.Until ) ) )
                {
                    effectiveEndDateTime = null;
                }
                else
                {
                    effectiveEndDateTime = endDateRules.Max( rule => rule.Until );
                }
            }

            if ( hasRuleWithCount )
            {
                if ( countRules.Any( rule => rule.Count > 999 ) && !hasRuleWithEndDate )
                {
                    // If there is a count rule greater than 999 (limit in the UI), and no end date rule was applied,
                    // we don't want to calculate occurrences because it will be too costly. Treat this as no end date.
                    effectiveEndDateTime = null;
                }
                else
                {
                    // This case means that there are count rules and they are <= 999. Go ahead and calculate the actual occurrences
                    // to get the EffectiveEndDate.
                    adjustEffectiveDateForLastOccurrence = true;
                }
            }

            // If specific recurrence dates exist, adjust the Effective End Date to the last specified date 
            // if it occurs after the Effective End Date required by the recurrence rules.
            if ( hasDates )
            {
                // If the Schedule does not have any other rules, reset the Effective End Date to the placeholder value
                // to ensure it is recalculated.
                if ( !hasRuleWithEndDate && !hasRuleWithCount )
                {
                    effectiveEndDateTime = DateTime.MaxValue;
                }

                adjustEffectiveDateForLastOccurrence = true;
            }

            if ( adjustEffectiveDateForLastOccurrence
                 && effectiveEndDateTime != null )
            {
                var occurrences = GetICalOccurrences( DateTime.MinValue, DateTime.MaxValue );

                if ( occurrences.Any() )
                {
                    var lastOccurrenceDate = occurrences.Any() // It is possible for an event to have no occurrences
                        ? occurrences.OrderByDescending( o => o.Period.StartTime.Date ).First().Period.EndTime.Value
                        : effectiveStartDateTime;

                    if ( effectiveEndDateTime == DateTime.MaxValue
                         || lastOccurrenceDate > effectiveEndDateTime )
                    {
                        effectiveEndDateTime = lastOccurrenceDate;
                    }
                }
            }

            // At this point, if no EffectiveEndDate is set then assume this is a one-time event and set the EffectiveEndDate to the EffectiveStartDate.
            if ( effectiveEndDateTime == DateTime.MaxValue && !adjustEffectiveDateForLastOccurrence )
            {
                effectiveEndDateTime = effectiveStartDateTime;
            }

            // Add the Duration of the event to ensure that the effective end date corresponds to the day on which the event concludes.
            if ( effectiveEndDateTime != null && effectiveEndDateTime != DateTime.MaxValue )
            {
                effectiveEndDateTime = effectiveEndDateTime.Value.AddMinutes( DurationInMinutes );
            }

            // Set the Effective Start and End dates. The dates are inclusive but do not have a time component.
            EffectiveStartDate = effectiveStartDateTime?.Date;
            EffectiveEndDate = effectiveEndDateTime?.Date;

            return ( EffectiveEndDate?.Date != originalEffectiveEndDate?.Date ) || ( EffectiveStartDate?.Date != originalEffectiveStartDate?.Date );
        }

        /// <summary>
        /// Gets the Schedule's iCalender Event.
        /// </summary>
        /// <value>
        /// A <see cref="DDay.iCal.Event"/> representing the iCalendar event for this Schedule.
        /// </value>
        [RockObsolete( "1.9" )]
        [Obsolete( "Use GetICalEvent() instead ", true )]
        public virtual DDay.iCal.Event GetCalenderEvent()
        {
            return ScheduleICalHelper.GetCalendarEvent( iCalendarContent );
        }

        /// <summary>
        /// Gets the Schedule's iCalender Event.
        /// </summary>
        /// <value>
        /// A <see cref="DDay.iCal.Event"/> representing the iCalendar event for this Schedule.
        /// </value>
        [RockObsolete( "1.12" )]
        [Obsolete( "Use GetICalEvent() instead " )]
        public virtual DDay.iCal.Event GetCalendarEvent()
        {
            return ScheduleICalHelper.GetCalendarEvent( iCalendarContent );
        }

        /// <summary>
        /// Gets the Schedule's iCalender Event.
        /// </summary>
        /// <value>
        /// A <see cref="Ical.Net.Event"/> representing the iCalendar event for this Schedule.
        /// </value>
        public virtual Ical.Net.Event GetICalEvent()
        {
            if ( _getICalEvent == null )
            {
                _getICalEvent = InetCalendarHelper.CreateCalendarEvent( iCalendarContent );
            }

            return _getICalEvent;
        }

        private Ical.Net.Event _getICalEvent = null;

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="beginDateTime">The begin date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        [RockObsolete( "1.12" )]
        [Obsolete( "Use GetICalOccurrences() instead." )]
        public IList<DDay.iCal.Occurrence> GetOccurrences( DateTime beginDateTime, DateTime? endDateTime = null )
        {
            return this.GetOccurrences( beginDateTime, endDateTime, null );
        }

        /// <summary>
        /// Gets the occurrences with option to override the ICal.Event.DTStart
        /// </summary>
        /// <param name="beginDateTime">The begin date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <param name="scheduleStartDateTimeOverride">The schedule start date time override.</param>
        /// <returns></returns>
        [RockObsolete( "1.12" )]
        [Obsolete( "Use GetICalOccurrences() instead." )]
        public IList<DDay.iCal.Occurrence> GetOccurrences( DateTime beginDateTime, DateTime? endDateTime, DateTime? scheduleStartDateTimeOverride )
        {
            var occurrences = new List<DDay.iCal.Occurrence>();

            DDay.iCal.Event calEvent = GetCalendarEvent();
            if ( calEvent == null )
            {
                return occurrences;
            }

            if ( scheduleStartDateTimeOverride.HasValue )
            {
                calEvent.DTStart = new DDay.iCal.iCalDateTime( scheduleStartDateTimeOverride.Value );
            }

            if ( calEvent.DTStart != null )
            {
                var exclusionDates = new List<DateRange>();
                if ( this.CategoryId.HasValue && this.CategoryId.Value > 0 )
                {
                    var category = CategoryCache.Get( this.CategoryId.Value );
                    if ( category != null )
                    {
                        exclusionDates = category.ScheduleExclusions
                            .Where( e => e.Start.HasValue && e.End.HasValue )
                            .ToList();
                    }
                }

                foreach ( var occurrence in endDateTime.HasValue ?
                    ScheduleICalHelper.GetOccurrences( calEvent, beginDateTime, endDateTime.Value ) :
                    ScheduleICalHelper.GetOccurrences( calEvent, beginDateTime ) )
                {
                    bool exclude = false;
                    if ( exclusionDates.Any() && occurrence.Period.StartTime != null )
                    {
                        var occurrenceStart = occurrence.Period.StartTime.Value;
                        if ( exclusionDates.Any( d =>
                            d.Start.Value <= occurrenceStart &&
                            d.End.Value >= occurrenceStart ) )
                        {
                            exclude = true;
                        }
                    }

                    if ( !exclude )
                    {
                        occurrences.Add( occurrence );
                    }
                }
            }

            return occurrences;
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="beginDateTime">The begin date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        public IList<Occurrence> GetICalOccurrences( DateTime beginDateTime, DateTime? endDateTime = null )
        {
            return this.GetICalOccurrences( beginDateTime, endDateTime, null );
        }

        /// <summary>
        /// Gets the occurrences with option to override the ICal.Event.DTStart
        /// </summary>
        /// <param name="beginDateTime">The begin date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <param name="scheduleStartDateTimeOverride">The schedule start date time override.</param>
        /// <returns></returns>
        public IList<Occurrence> GetICalOccurrences( DateTime beginDateTime, DateTime? endDateTime, DateTime? scheduleStartDateTimeOverride )
        {
            var occurrences = new List<Occurrence>();

            DateTime? scheduleStartDateTime;

            if ( scheduleStartDateTimeOverride.HasValue )
            {
                scheduleStartDateTime = scheduleStartDateTimeOverride;
            }
            else
            {
                Event calEvent = GetICalEvent();
                if ( calEvent == null )
                {
                    return occurrences;
                }

                scheduleStartDateTime = calEvent.DtStart?.Value;
            }

            if ( scheduleStartDateTime != null )
            {
                var exclusionDates = new List<DateRange>();
                if ( this.CategoryId.HasValue && this.CategoryId.Value > 0 )
                {
                    var category = CategoryCache.Get( this.CategoryId.Value );
                    if ( category != null )
                    {
                        exclusionDates = category.ScheduleExclusions
                            .Where( e => e.Start.HasValue && e.End.HasValue )
                            .ToList();
                    }
                }

                foreach ( var occurrence in InetCalendarHelper.GetOccurrences( iCalendarContent, beginDateTime, endDateTime, scheduleStartDateTimeOverride ) )
                {
                    bool exclude = false;
                    if ( exclusionDates.Any() && occurrence.Period.StartTime != null )
                    {
                        var occurrenceStart = occurrence.Period.StartTime.Value;
                        if ( exclusionDates.Any( d =>
                            d.Start.Value <= occurrenceStart &&
                            d.End.Value >= occurrenceStart ) )
                        {
                            exclude = true;
                        }
                    }

                    if ( !exclude )
                    {
                        occurrences.Add( occurrence );
                    }
                }
            }

            return occurrences;
        }

        /// <summary>
        /// Gets the check in times.
        /// </summary>
        /// <param name="beginDateTime">The begin date time.</param>
        /// <returns></returns>
        public virtual List<CheckInTimes> GetCheckInTimes( DateTime beginDateTime )
        {
            var result = new List<CheckInTimes>();

            if ( IsCheckInEnabled )
            {
                var occurrences = GetICalOccurrences( beginDateTime, beginDateTime.Date.AddDays( 1 ) );
                foreach ( var occurrence in occurrences
                    .Where( a =>
                        a.Period != null &&
                        a.Period.StartTime != null &&
                        a.Period.EndTime != null )
                    .Select( a => new
                    {
                        Start = a.Period.StartTime.Value,
                        End = a.Period.EndTime.Value
                    } ) )
                {
                    var checkInTimes = new CheckInTimes();
                    checkInTimes.Start = DateTime.SpecifyKind( occurrence.Start, DateTimeKind.Local );
                    checkInTimes.End = DateTime.SpecifyKind( occurrence.End, DateTimeKind.Local );
                    checkInTimes.CheckInStart = checkInTimes.Start.AddMinutes( 0 - CheckInStartOffsetMinutes.Value );
                    if ( CheckInEndOffsetMinutes.HasValue )
                    {
                        checkInTimes.CheckInEnd = checkInTimes.Start.AddMinutes( CheckInEndOffsetMinutes.Value );
                    }
                    else
                    {
                        checkInTimes.CheckInEnd = checkInTimes.End;
                    }

                    result.Add( checkInTimes );
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the next check in start time.
        /// </summary>
        /// <param name="begindateTime">The begindate time.</param>
        /// <returns></returns>
        public virtual DateTime? GetNextCheckInStartTime( DateTime begindateTime )
        {
            var checkInTimes = GetCheckInTimes( begindateTime );
            if ( checkInTimes != null && checkInTimes.Any() )
            {
                return checkInTimes.FirstOrDefault().CheckInStart;
            }

            return null;
        }

        /// <summary>
        /// Gets a list of scheduled start datetimes between the two specified dates, sorted by datetime.
        /// </summary>
        /// <param name="beginDateTime">The begin date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        public virtual List<DateTime> GetScheduledStartTimes( DateTime beginDateTime, DateTime endDateTime )
        {
            var result = new List<DateTime>();

            var occurrences = GetICalOccurrences( beginDateTime, endDateTime );
            foreach ( var startDateTime in occurrences
                .Where( a =>
                    a.Period != null &&
                    a.Period.StartTime != null )
                .Select( a => a.Period.StartTime.Value ) )
            {
                // ensure the datetime is DateTimeKind.Local since iCal returns DateTimeKind.UTC
                result.Add( DateTime.SpecifyKind( startDateTime, DateTimeKind.Local ) );
            }

            return result;
        }

        /// <summary>
        /// Gets the first start date time.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime? GetFirstStartDateTime()
        {
            DateTime? firstStartTime = null;

            if ( this.EffectiveStartDate.HasValue )
            {
                var scheduledStartTimes = this.GetScheduledStartTimes( this.EffectiveStartDate.Value, this.EffectiveStartDate.Value.AddMonths( 1 ) );
                if ( scheduledStartTimes.Count > 0 )
                {
                    firstStartTime = scheduledStartTimes[0];
                }
            }

            return firstStartTime;
        }

        /// <summary>
        /// Determines whether this instance has a non-empty schedule.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasSchedule()
        {
            var calEvent = GetICalEvent();
            if ( calEvent != null && calEvent.DtStart != null )
            {
                return true;
            }
            else
            {
                // if there is no CalEvent, it might be scheduled using WeeklyDayOfWeek
                return WeeklyDayOfWeek.HasValue;
            }
        }

        /// <summary>
        /// returns true if there is a blank schedule or a schedule that is incomplete
        /// </summary>
        /// <returns></returns>
        public virtual bool HasScheduleWarning()
        {
            var calEvent = GetICalEvent();
            if ( calEvent != null && calEvent.DtStart != null )
            {
                if ( calEvent.RecurrenceRules.Any() )
                {
                    var rrule = calEvent.RecurrenceRules[0];
                    if ( rrule.Frequency == FrequencyType.Weekly )
                    {
                        // if it has a Weekly schedule, but no days are selected, return true that is has a warning
                        if ( !rrule.ByDay.Any() )
                        {
                            return true;
                        }
                    }
                    else if ( rrule.Frequency == FrequencyType.Monthly )
                    {
                        // if it has a Monthly schedule, but not configured, return true that is has a warning
                        if ( !rrule.ByDay.Any() && !rrule.ByMonthDay.Any() )
                        {
                            return true;
                        }
                    }
                }

                // is scheduled, and doesn't have any warnings
                return false;
            }
            else
            {
                // if there is no CalEvent, it might be scheduled using WeeklyDayOfWeek, but if it isn't, return true that is has a warning
                return !WeeklyDayOfWeek.HasValue;
            }
        }

        /// <summary>
        /// Gets the Friendly Text of the Calendar Event.
        /// For example, "Every 3 days at 10:30am", "Monday, Wednesday, Friday at 5:00pm", "Saturday at 4:30pm"
        /// </summary>
        /// <returns>A <see cref="System.String"/> containing a friendly description of the Schedule.</returns>
        public string ToFriendlyScheduleText()
        {
            return ToFriendlyScheduleText( false );
        }

        /// <summary>
        /// Gets the Friendly Text of the Calendar Event.
        /// For example, "Every 3 days at 10:30am", "Monday, Wednesday, Friday at 5:00pm", "Saturday at 4:30pm"
        /// </summary>
        /// <param name="condensed">if set to <c>true</c> [condensed].</param>
        /// <returns>
        /// A <see cref="System.String" /> containing a friendly description of the Schedule.
        /// </returns>
        public string ToFriendlyScheduleText( bool condensed )
        {
            // init the result to just the schedule name just in case we can't figure out the FriendlyText
            string result = this.Name;

            var calendarEvent = GetICalEvent();
            if ( calendarEvent != null && calendarEvent.DtStart != null )
            {
                string startTimeText = calendarEvent.DtStart.Value.TimeOfDay.ToTimeString();
                if ( calendarEvent.RecurrenceRules.Any() )
                {
                    // some type of recurring schedule
                    var rrule = calendarEvent.RecurrenceRules[0];
                    switch ( rrule.Frequency )
                    {
                        case FrequencyType.Daily:
                            result = "Daily";

                            if ( rrule.Interval > 1 )
                            {
                                result += string.Format( " every {0} days", rrule.Interval );
                            }

                            result += " at " + startTimeText;

                            break;

                        case FrequencyType.Weekly:

                            result = rrule.ByDay.Select( a => a.DayOfWeek.ConvertToString().Pluralize() ).ToList().AsDelimited( "," );
                            if ( string.IsNullOrEmpty( result ) )
                            {
                                // no day selected, so it has an incomplete schedule
                                return "No Scheduled Days";
                            }

                            if ( rrule.Interval > 1 )
                            {
                                result = string.Format( "Every {0} weeks: ", rrule.Interval ) + result;
                            }
                            else
                            {
                                result = "Weekly: " + result;
                            }

                            result += " at " + startTimeText;

                            break;

                        case FrequencyType.Monthly:

                            if ( rrule.ByMonthDay.Count > 0 )
                            {
                                // Day X of every X Months (we only support one day in the ByMonthDay list)
                                int monthDay = rrule.ByMonthDay[0];
                                result = string.Format( "Day {0} of every ", monthDay );
                                if ( rrule.Interval > 1 )
                                {
                                    result += string.Format( "{0} months", rrule.Interval );
                                }
                                else
                                {
                                    result += "month";
                                }

                                result += " at " + startTimeText;
                            }
                            else if ( rrule.ByDay.Count > 0 )
                            {
                                // The Nth <DayOfWeekName>.  We only support one *day* in the ByDay list, but multiple *offsets*.
                                // So, it can be the "The first and third Monday" of every month.
                                var bydate = rrule.ByDay[0];
                                var offsetNames = NthNamesAbbreviated.Where( a => rrule.ByDay.Select( o => o.Offset ).Contains( a.Key ) ).Select( a => a.Value );
                                if ( offsetNames != null )
                                {
                                    result = string.Format( "The {0} {1} of every month", offsetNames.JoinStringsWithCommaAnd(), bydate.DayOfWeek.ConvertToString() );
                                }
                                else
                                {
                                    // unsupported case (just show the name)
                                }

                                result += " at " + startTimeText;
                            }
                            else
                            {
                                // unsupported case (just show the name)
                            }

                            break;

                        default:
                            // some other type of recurring type (probably specific dates).  Just return the Name of the schedule
                            break;
                    }
                }
                else
                {
                    // not any type of recurring, might be one-time or from specific dates, etc
                    var dates = InetCalendarHelper.GetOccurrences( iCalendarContent, DateTime.MinValue, DateTime.MaxValue, null )
                        .Where( a => a.Period != null && a.Period.StartTime != null )
                        .Select( a => a.Period.StartTime.Value )
                        .OrderBy( a => a ).ToList();

                    if ( dates.Count() > 1 )
                    {
                        if ( condensed || dates.Count() > 99 )
                        {
                            result = string.Format( "Multiple dates between {0} and {1}", dates.First().ToShortDateString(), dates.Last().ToShortDateString() );
                        }
                        else
                        {
                            var listHtml = "<ul class='list-unstyled'>" + Environment.NewLine;
                            foreach ( var date in dates )
                            {
                                listHtml += string.Format( "<li>{0}</li>", date.ToShortDateTimeString() ) + Environment.NewLine;
                            }

                            listHtml += "</ul>";

                            result = listHtml;
                        }
                    }
                    else if ( dates.Count() == 1 )
                    {
                        result = "Once at " + calendarEvent.DtStart.Value.ToShortDateTimeString();
                    }
                    else
                    {
                        return "No Schedule";
                    }
                }
            }
            else
            {
                if ( WeeklyDayOfWeek.HasValue )
                {
                    result = WeeklyDayOfWeek.Value.ConvertToString();
                    if ( WeeklyTimeOfDay.HasValue )
                    {
                        result += " at " + WeeklyTimeOfDay.Value.ToTimeString();
                    }
                }
                else
                {
                    // no start time.  Nothing scheduled
                    return "No Schedule";
                }
            }

            return result;
        }

        /// <summary>
        /// Returns value indicating if the schedule was active at the specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public bool WasScheduleActive( DateTime time )
        {
            var calEvent = this.GetICalEvent();
            if ( calEvent != null && calEvent.DtStart != null )
            {
                // Is the current time earlier than the event's start time?
                if ( time.TimeOfDay.TotalSeconds < calEvent.DtStart.Value.TimeOfDay.TotalSeconds )
                {
                    return false;
                }

                // Is the current time later than the event's end time?
                if ( time.TimeOfDay.TotalSeconds > calEvent.DtEnd.Value.TimeOfDay.TotalSeconds )
                {
                    return false;
                }

                var occurrences = GetICalOccurrences( time.Date );
                return occurrences.Count > 0;
            }

            return false;
        }

        /// <summary>
        /// Returns value indicating if check-in was active at the specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public bool WasCheckInActive( DateTime time )
        {
            if ( !IsCheckInEnabled )
            {
                return false;
            }

            var calEvent = this.GetICalEvent();
            if ( calEvent != null && calEvent.DtStart != null )
            {
                // Is the current time earlier the event's allowed check-in window?
                var checkInStart = calEvent.DtStart.AddMinutes( 0 - CheckInStartOffsetMinutes.Value );
                if ( time.TimeOfDay.TotalSeconds < checkInStart.Value.TimeOfDay.TotalSeconds )
                {
                    return false;
                }

                var checkInEnd = calEvent.DtEnd;
                if ( CheckInEndOffsetMinutes.HasValue )
                {
                    checkInEnd = calEvent.DtStart.AddMinutes( CheckInEndOffsetMinutes.Value );
                }

                // If compare is greater than zero, then check-in offset end resulted in an end time in next day, in 
                // which case, don't need to compare time
                int checkInEndDateCompare = checkInEnd.Date.CompareTo( checkInStart.Date );

                if ( checkInEndDateCompare < 0 )
                {
                    // End offset is prior to start (Would have required a neg number entered)
                    return false;
                }

                // Is the current time later then the event's allowed check-in window?
                if ( checkInEndDateCompare == 0 && time.TimeOfDay.TotalSeconds > checkInEnd.Value.TimeOfDay.TotalSeconds )
                {
                    // Same day, but end time has passed
                    return false;
                }

                var occurrences = GetICalOccurrences( time.Date );
                return occurrences.Count > 0;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a schedule is active for check-out for the specified time.
        /// </summary>
        /// <example>
        /// CheckOut Window: 5/1/2013 11:00:00 PM - 5/2/2013 2:00:00 AM
        /// 
        ///  * Current time: 8/8/2019 11:01:00 PM - returns true
        ///  * Current time: 8/8/2019 10:59:00 PM - returns false
        ///  * Current time: 8/8/2019 1:00:00 AM - returns true
        ///  * Current time: 8/8/2019 2:01:00 AM - returns false
        ///
        /// Note: Add any other test cases you want to test to the "Rock.Tests.Rock.Model.ScheduleCheckInTests" project.
        /// </example>
        /// <param name="time">The time.</param>
        /// <returns>
        ///   <c>true</c> if the schedule is active for check out at the specified time; otherwise, <c>false</c>.
        /// </returns>
        private bool IsScheduleActiveForCheckOut( DateTime time )
        {
            var calEvent = GetICalEvent();
            if ( calEvent == null || calEvent.DtStart == null )
            {
                return false;
            }

            // For check-out, we use the start time + duration to determine the end of the window...
            // ...in iCal, this is the DTEnd value
            var checkOutEnd = calEvent.DtEnd;
            var checkInStart = calEvent.DtStart.AddMinutes( 0 - CheckInStartOffsetMinutes.Value );

            // Check if the end time spilled over to a different day...
            int checkOutEndDateCompare = checkOutEnd.Date.CompareTo( checkInStart.Date );

            if ( checkOutEndDateCompare < 0 )
            {
                // invalid condition, end before the start
                return false;
            }
            else if ( checkOutEndDateCompare == 0 )
            {
                // the start and end are on the same day, so we can do simple time checking
                // Is the current time earlier the event's allowed check-in window?
                if ( time.TimeOfDay.TotalSeconds < checkInStart.Value.TimeOfDay.TotalSeconds )
                {
                    // Same day, but it's too early
                    return false;
                }

                // Is the current time later than the event's allowed check-in window?
                if ( time.TimeOfDay.TotalSeconds > checkOutEnd.Value.TimeOfDay.TotalSeconds )
                {
                    // Same day, but end time has passed
                    return false;
                }
            }
            else if ( checkOutEndDateCompare > 0 )
            {
                // Does the end time spill over to a different day...
                // if so, we have to look for crossover conditions

                // The current time is before the start time and later than the end time:
                // ex: 11PM-2AM window, and it's 10PM -- not in the window
                // ex: 11PM-2AM window, and it's 3AM -- not in the window
                if ( time.TimeOfDay.TotalSeconds < checkInStart.Value.TimeOfDay.TotalSeconds && time.TimeOfDay.TotalSeconds > checkOutEnd.AsSystemLocal.TimeOfDay.TotalSeconds )
                {
                    return false;
                }
            }

            var occurrences = GetICalOccurrences( time.Date );
            return occurrences.Count > 0;
        }

        /// <summary>
        /// Determines if the schedule or check in is active for check out.
        /// Check-out can happen while check-in is active or until the event
        /// ends (start time + duration).
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public bool WasScheduleOrCheckInActiveForCheckOut( DateTime time )
        {
            return IsScheduleActiveForCheckOut( time ) || WasScheduleActive( time ) || WasCheckInActive( time );
        }

        /// <summary>
        /// Returns value indicating if check-in was active at a current time for this schedule.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public bool WasScheduleOrCheckInActive( DateTime time )
        {
            return WasScheduleActive( time ) || WasCheckInActive( time );
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if ( this.Name.IsNotNullOrWhiteSpace() )
            {
                return this.Name;
            }
            else
            {
                return this.ToFriendlyScheduleText();
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// The "nth" names for DayName of Month (First, Second, Third, Forth, Last)
        /// </summary>
        public static readonly Dictionary<int, string> NthNames = new Dictionary<int, string> {
            { 1, "First" },
            { 2, "Second" },
            { 3, "Third" },
            { 4, "Fourth" },
            { -1, "Last" }
        };

        /// <summary>
        /// The abbreviated "nth" names for DayName of Month (1st, 2nd, 3rd, 4th, last)
        /// </summary>
        private static readonly Dictionary<int, string> NthNamesAbbreviated = new Dictionary<int, string> {
            { 1, "1st" },
            { 2, "2nd" },
            { 3, "3rd" },
            { 4, "4th" },
            { -1, "last" }
        };

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// File Configuration class.
    /// </summary>
    public partial class ScheduleConfiguration : EntityTypeConfiguration<Schedule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleConfiguration"/> class.
        /// </summary>
        public ScheduleConfiguration()
        {
            this.HasOptional( s => s.Category ).WithMany().HasForeignKey( s => s.CategoryId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

    #region Enumerations

    /// <summary>
    /// Schedule Type
    /// </summary>
    [Flags]
    public enum ScheduleType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Weekly
        /// </summary>
        Weekly = 1,

        /// <summary>
        /// Custom
        /// </summary>
        Custom = 2,

        /// <summary>
        /// Custom
        /// </summary>
        Named = 4,
    }

    #endregion

    #region Helper Classes

    /// <summary>
    /// Start/End Times for Check-in
    /// </summary>
    public class CheckInTimes
    {
        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the check in start.
        /// </summary>
        /// <value>
        /// The check in start.
        /// </value>
        public DateTime CheckInStart { get; set; }

        /// <summary>
        /// Gets or sets the check in end.
        /// </summary>
        /// <value>
        /// The check in end.
        /// </value>
        public DateTime CheckInEnd { get; set; }
    }

    /// <summary>
    /// Helper class for grouping attendance records associated into logical occurrences based on
    /// a given schedule
    /// </summary>
    public class ScheduleOccurrence
    {
        /// <summary>
        /// Gets or sets the logical occurrence date of the occurrence
        /// </summary>
        /// <value>
        /// The occurrence date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the logical start date/time ( only used for ordering )
        /// </summary>
        /// <value>
        /// The start date time.
        /// </value>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the schedule identifier.
        /// </summary>
        /// <value>
        /// The schedule identifier.
        /// </value>
        public int? ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the schedule.
        /// </summary>
        /// <value>
        /// The name of the schedule.
        /// </value>
        public string ScheduleName { get; set; }

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int? LocationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the location path.
        /// </summary>
        /// <value>
        /// The location path.
        /// </value>
        public string LocationPath { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the did attend count.
        /// </summary>
        /// <value>
        /// The did attend count.
        /// </value>
        public int DidAttendCount { get; set; }

        /// <summary>
        /// Gets or sets the did not occur count.
        /// </summary>
        /// <value>
        /// The did not occur count.
        /// </value>
        public int DidNotOccurCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether attendance has been entered for this occurrence.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [attendance entered]; otherwise, <c>false</c>.
        /// </value>
        public bool AttendanceEntered
        {
            get
            {
                return DidAttendCount > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether occurrence did not occur for the selected
        /// start time. This is determined by not having any attendance records with 
        /// a 'DidAttend' value, and at least one attendance record with 'DidNotOccur'
        /// value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [did not occur]; otherwise, <c>false</c>.
        /// </value>
        public bool DidNotOccur
        {
            get
            {
                return DidAttendCount <= 0 && DidNotOccurCount > 0;
            }
        }

        /// <summary>
        /// Gets the attendance percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public double Percentage
        {
            get
            {
                if ( TotalCount > 0 )
                {
                    return DidAttendCount / ( double ) TotalCount;
                }
                else
                {
                    return 0.0d;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleOccurrence" /> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="scheduleId">The schedule identifier.</param>
        /// <param name="scheduleName">Name of the schedule.</param>
        /// <param name="locationId">The location identifier.</param>
        /// <param name="locationName">Name of the location.</param>
        /// <param name="locationPath">The location path.</param>
        /// ,
        public ScheduleOccurrence( DateTime date, TimeSpan startTime, int? scheduleId = null, string scheduleName = "", int? locationId = null, string locationName = "", string locationPath = "" )
        {
            Date = date;
            StartTime = startTime;
            ScheduleId = scheduleId;
            ScheduleName = scheduleName;
            LocationId = locationId;
            LocationName = locationName;
            LocationPath = locationPath;
        }
    }

    /// <summary>
    /// A helper class for processing iCalendar (RFC 5545) schedules.
    /// </summary>
    /// <remarks>
    /// This class uses the iCal.Net implementation of the iCalendar (RFC 5545) standard.
    /// </remarks>
    public static class InetCalendarHelper
    {
        // using MemoryCache instead RockCacheManager, since Occurrences isn't serializable.
        private static MemoryCache _iCalOccurrencesCache = new MemoryCache( "Rock.InetCalendarHelper._iCalOccurrences" );

        // only keep in memory if unused for 10 minutes. This reduces the chances of this getting too big.
        private static CacheItemPolicy cacheItemPolicy10Minutes = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes( 10 ) };

        /// <summary>
        /// Gets the calendar event.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <returns></returns>
        [RockObsolete( "1.12.4" )]
        [Obsolete( "Use CreateCalendarEvent instead" )]
        public static Ical.Net.Event GetCalendarEvent( string iCalendarContent )
        {
            // changed to obsolete because this used to return a shared object that could be altered or create thread-safety issues
            return CreateCalendarEvent( iCalendarContent );
        }

        /// <summary>
        /// Creates the calendar event.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <returns></returns>
        public static Event CreateCalendarEvent( string iCalendarContent )
        {
            StringReader stringReader = new StringReader( iCalendarContent );
            var calendarList = Calendar.LoadFromStream( stringReader );
            Event calendarEvent = null;

            //// iCal is stored as a list of Calendar's each with a list of Events, etc.  
            //// We just need one Calendar and one Event
            if ( calendarList.Count() > 0 )
            {
                var calendar = calendarList[0] as Calendar;
                if ( calendar != null )
                {
                    calendarEvent = calendar.Events[0] as Event;
                }
            }

            return calendarEvent;
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="icalEvent">The ical event.</param>
        /// <param name="startTime">The start time.</param>
        /// <returns></returns>
        [Obsolete( "Use the override with the string instead of the Ical.Net.Event." )]
        [RockObsolete( "1.12.4" )]
        public static IList<Occurrence> GetOccurrences( Ical.Net.Event icalEvent, DateTime startTime )
        {
            return icalEvent.GetOccurrences( startTime ).ToList();
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="icalEvent">The ical event.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns></returns>
        [Obsolete( "Use the override with the string instead of the Ical.Net.Event." )]
        [RockObsolete( "1.12.4" )]
        public static IList<Occurrence> GetOccurrences( Ical.Net.Event icalEvent, DateTime startTime, DateTime endTime )
        {
            return icalEvent.GetOccurrences( startTime, endTime ).ToList();
        }

        /// <summary>
        /// Gets the occurrences for the specified iCal
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <returns></returns>
        public static IList<Occurrence> GetOccurrences( string iCalendarContent, DateTime startDateTime )
        {
            return GetOccurrences( iCalendarContent, startDateTime, null );
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        public static IList<Occurrence> GetOccurrences( string iCalendarContent, DateTime startDateTime, DateTime? endDateTime )
        {
            return GetOccurrences( iCalendarContent, startDateTime, endDateTime, null );
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <param name="scheduleStartDateTimeOverride">The schedule start date time override.</param>
        /// <returns></returns>
        public static IList<Occurrence> GetOccurrences( string iCalendarContent, DateTime startDateTime, DateTime? endDateTime, DateTime? scheduleStartDateTimeOverride )
        {
            string occurrenceLookupKey = $"{startDateTime.ToShortDateTimeString()}__{endDateTime?.ToShortDateTimeString()}__{scheduleStartDateTimeOverride?.ToShortDateTimeString()}__{iCalendarContent.Trim()}".XxHash();

            Occurrence[] occurrenceList = _iCalOccurrencesCache.Get( occurrenceLookupKey ) as Occurrence[];

            if ( occurrenceList == null )
            {
                occurrenceList = LoadOccurrences( iCalendarContent, startDateTime, endDateTime, scheduleStartDateTimeOverride );
                _iCalOccurrencesCache.AddOrGetExisting( occurrenceLookupKey, occurrenceList, cacheItemPolicy10Minutes );
            }

            return occurrenceList;
        }

        /// <summary>
        /// Loads the occurrences.
        /// </summary>
        /// <param name="iCalendarContent">RFC 5545 ICal Content</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <param name="scheduleStartDateTimeOverride">The schedule start date time override.</param>
        /// <returns></returns>
        private static Occurrence[] LoadOccurrences( string iCalendarContent, DateTime startDateTime, DateTime? endDateTime, DateTime? scheduleStartDateTimeOverride )
        {
            var iCalEvent = CreateCalendarEvent( iCalendarContent );
            if ( iCalEvent == null )
            {
                return new Occurrence[0];
            }

            if ( scheduleStartDateTimeOverride.HasValue )
            {
                iCalEvent.DtStart = new CalDateTime( scheduleStartDateTimeOverride.Value );
            }

            if ( endDateTime.HasValue )
            {
                return iCalEvent.GetOccurrences( startDateTime, endDateTime.Value ).ToArray();
            }
            else
            {
                return iCalEvent.GetOccurrences( startDateTime ).ToArray();
            }
        }
    }

    #region Obsolete Code

    /// <summary>
    /// DDay.ical LoadFromStream is not threadsafe, so use locking
    /// </summary>
    [RockObsolete( "1.12" )]
    [Obsolete( "Use InetCalendarHelper instead." )]
    public static class ScheduleICalHelper
    {
        private static object _initLock;
        private static Dictionary<string, DDay.iCal.Event> _iCalSchedules = new Dictionary<string, DDay.iCal.Event>();

        static ScheduleICalHelper()
        {
            ScheduleICalHelper._initLock = new object();
        }

        /// <summary>
        /// Gets the calendar event.
        /// </summary>
        /// <param name="iCalendarContent">Content of the i calendar.</param>
        /// <returns></returns>
        [RockObsolete( "1.9" )]
        [Obsolete( "Use GetCalendarEvent( iCalendarContent ) instead ", true )]
        public static DDay.iCal.Event GetCalenderEvent( string iCalendarContent )
        {
            return GetCalendarEvent( iCalendarContent );
        }

        /// <summary>
        /// Gets the calendar event.
        /// </summary>
        /// <param name="iCalendarContent">Content of the i calendar.</param>
        /// <returns></returns>
        public static DDay.iCal.Event GetCalendarEvent( string iCalendarContent )
        {
            string trimmedContent = iCalendarContent.Trim();

            if ( string.IsNullOrWhiteSpace( trimmedContent ) )
            {
                return null;
            }

            DDay.iCal.Event calendarEvent = null;

            lock ( ScheduleICalHelper._initLock )
            {
                if ( _iCalSchedules.ContainsKey( trimmedContent ) )
                {
                    return _iCalSchedules[trimmedContent];
                }

                StringReader stringReader = new StringReader( trimmedContent );
                var calendarList = DDay.iCal.iCalendar.LoadFromStream( stringReader );

                //// iCal is stored as a list of Calendar's each with a list of Events, etc.  
                //// We just need one Calendar and one Event
                if ( calendarList.Count > 0 )
                {
                    var calendar = calendarList[0] as DDay.iCal.iCalendar;
                    if ( calendar != null )
                    {
                        calendarEvent = calendar.Events[0] as DDay.iCal.Event;
                        _iCalSchedules.AddOrReplace( trimmedContent, calendarEvent );
                    }
                }
            }

            return calendarEvent;
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="icalEvent">The ical event.</param>
        /// <param name="startTime">The start time.</param>
        /// <returns></returns>
        public static IList<DDay.iCal.Occurrence> GetOccurrences( DDay.iCal.Event icalEvent, DateTime startTime )
        {
            lock ( ScheduleICalHelper._initLock )
            {
                return icalEvent.GetOccurrences( startTime );
            }
        }

        /// <summary>
        /// Gets the occurrences.
        /// </summary>
        /// <param name="icalEvent">The ical event.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns></returns>
        public static IList<DDay.iCal.Occurrence> GetOccurrences( DDay.iCal.Event icalEvent, DateTime startTime, DateTime endTime )
        {
            lock ( ScheduleICalHelper._initLock )
            {
                return icalEvent.GetOccurrences( startTime, endTime );
            }
        }
    }

    #endregion

    #endregion
}