//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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

using System;
using System.Linq;

using Rock.Attribute;
using Rock.Data;
using Rock.ViewModel;
using Rock.Web.Cache;

namespace Rock.Model
{
    /// <summary>
    /// DefinedValue Service class
    /// </summary>
    public partial class DefinedValueService : Service<DefinedValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedValueService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public DefinedValueService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( DefinedValue item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new Service<Attendance>( Context ).Queryable().Any( a => a.DeclineReasonValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Attendance.FriendlyTypeName );
                return false;
            }

            if ( new Service<Attendance>( Context ).Queryable().Any( a => a.QualifierValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Attendance.FriendlyTypeName );
                return false;
            }

            if ( new Service<Attendance>( Context ).Queryable().Any( a => a.SearchTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Attendance.FriendlyTypeName );
                return false;
            }

            if ( new Service<AttendanceOccurrence>( Context ).Queryable().Any( a => a.AttendanceTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, AttendanceOccurrence.FriendlyTypeName );
                return false;
            }

            if ( new Service<BenevolenceRequest>( Context ).Queryable().Any( a => a.ConnectionStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, BenevolenceRequest.FriendlyTypeName );
                return false;
            }

            if ( new Service<BenevolenceRequest>( Context ).Queryable().Any( a => a.RequestStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, BenevolenceRequest.FriendlyTypeName );
                return false;
            }

            if ( new Service<BenevolenceResult>( Context ).Queryable().Any( a => a.ResultTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, BenevolenceResult.FriendlyTypeName );
                return false;
            }

            if ( new Service<Campus>( Context ).Queryable().Any( a => a.CampusStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Campus.FriendlyTypeName );
                return false;
            }

            if ( new Service<Campus>( Context ).Queryable().Any( a => a.CampusTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Campus.FriendlyTypeName );
                return false;
            }

            if ( new Service<CampusSchedule>( Context ).Queryable().Any( a => a.ScheduleTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, CampusSchedule.FriendlyTypeName );
                return false;
            }

            if ( new Service<Communication>( Context ).Queryable().Any( a => a.SMSFromDefinedValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Communication.FriendlyTypeName );
                return false;
            }

            if ( new Service<CommunicationTemplate>( Context ).Queryable().Any( a => a.SMSFromDefinedValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, CommunicationTemplate.FriendlyTypeName );
                return false;
            }

            if ( new Service<ContentChannel>( Context ).Queryable().Any( a => a.StructuredContentToolValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, ContentChannel.FriendlyTypeName );
                return false;
            }

            if ( new Service<Device>( Context ).Queryable().Any( a => a.DeviceTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Device.FriendlyTypeName );
                return false;
            }

            if ( new Service<EntitySet>( Context ).Queryable().Any( a => a.EntitySetPurposeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, EntitySet.FriendlyTypeName );
                return false;
            }

            if ( new Service<EventItemAudience>( Context ).Queryable().Any( a => a.DefinedValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, EventItemAudience.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialAccount>( Context ).Queryable().Any( a => a.AccountTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialAccount.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialPaymentDetail>( Context ).Queryable().Any( a => a.CreditCardTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialPaymentDetail.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialPaymentDetail>( Context ).Queryable().Any( a => a.CurrencyTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialPaymentDetail.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialPledge>( Context ).Queryable().Any( a => a.PledgeFrequencyValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialPledge.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialScheduledTransaction>( Context ).Queryable().Any( a => a.ForeignCurrencyCodeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialScheduledTransaction.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialScheduledTransaction>( Context ).Queryable().Any( a => a.SourceTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialScheduledTransaction.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialScheduledTransaction>( Context ).Queryable().Any( a => a.TransactionFrequencyValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialScheduledTransaction.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialScheduledTransaction>( Context ).Queryable().Any( a => a.TransactionTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialScheduledTransaction.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialTransaction>( Context ).Queryable().Any( a => a.NonCashAssetTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialTransaction.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialTransaction>( Context ).Queryable().Any( a => a.SourceTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialTransaction.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialTransaction>( Context ).Queryable().Any( a => a.TransactionTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialTransaction.FriendlyTypeName );
                return false;
            }

            if ( new Service<FinancialTransactionRefund>( Context ).Queryable().Any( a => a.RefundReasonValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialTransactionRefund.FriendlyTypeName );
                return false;
            }

            if ( new Service<Group>( Context ).Queryable().Any( a => a.InactiveReasonValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Group.FriendlyTypeName );
                return false;
            }

            if ( new Service<Group>( Context ).Queryable().Any( a => a.StatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Group.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupLocation>( Context ).Queryable().Any( a => a.GroupLocationTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, GroupLocation.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupType>( Context ).Queryable().Any( a => a.GroupTypePurposeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }

            if ( new Service<InteractionChannel>( Context ).Queryable().Any( a => a.ChannelTypeMediumValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, InteractionChannel.FriendlyTypeName );
                return false;
            }

            if ( new Service<Location>( Context ).Queryable().Any( a => a.LocationTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Location.FriendlyTypeName );
                return false;
            }

            if ( new Service<Metric>( Context ).Queryable().Any( a => a.SourceValueTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Metric.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.ConnectionStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.MaritalStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.PreferredLanguageValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.RecordStatusReasonValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.RecordStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.RecordTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.ReviewReasonValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.SuffixValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<Person>( Context ).Queryable().Any( a => a.TitleValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }

            if ( new Service<PersonalDevice>( Context ).Queryable().Any( a => a.PersonalDeviceTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, PersonalDevice.FriendlyTypeName );
                return false;
            }

            if ( new Service<PersonSearchKey>( Context ).Queryable().Any( a => a.SearchTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, PersonSearchKey.FriendlyTypeName );
                return false;
            }

            if ( new Service<PhoneNumber>( Context ).Queryable().Any( a => a.NumberTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, PhoneNumber.FriendlyTypeName );
                return false;
            }

            if ( new Service<PrayerRequest>( Context ).Queryable().Any( a => a.LanguageValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, PrayerRequest.FriendlyTypeName );
                return false;
            }

            if ( new Service<WorkflowActionForm>( Context ).Queryable().Any( a => a.PersonEntryCampusStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, WorkflowActionForm.FriendlyTypeName );
                return false;
            }

            if ( new Service<WorkflowActionForm>( Context ).Queryable().Any( a => a.PersonEntryCampusTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, WorkflowActionForm.FriendlyTypeName );
                return false;
            }

            if ( new Service<WorkflowActionForm>( Context ).Queryable().Any( a => a.PersonEntryConnectionStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, WorkflowActionForm.FriendlyTypeName );
                return false;
            }

            if ( new Service<WorkflowActionForm>( Context ).Queryable().Any( a => a.PersonEntryGroupLocationTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, WorkflowActionForm.FriendlyTypeName );
                return false;
            }

            if ( new Service<WorkflowActionForm>( Context ).Queryable().Any( a => a.PersonEntryRecordStatusValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, WorkflowActionForm.FriendlyTypeName );
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// DefinedValue View Model Helper
    /// </summary>
    [DefaultViewModelHelper( typeof( DefinedValue ) )]
    public partial class DefinedValueViewModelHelper : ViewModelHelper<DefinedValue, Rock.ViewModel.DefinedValueViewModel>
    {
        /// <summary>
        /// Converts the model to a view model.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.DefinedValueViewModel CreateViewModel( DefinedValue model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.DefinedValueViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                DefinedTypeId = model.DefinedTypeId,
                Description = model.Description,
                IsActive = model.IsActive,
                IsSystem = model.IsSystem,
                Order = model.Order,
                Value = model.Value,
                CreatedDateTime = model.CreatedDateTime,
                ModifiedDateTime = model.ModifiedDateTime,
                CreatedByPersonAliasId = model.CreatedByPersonAliasId,
                ModifiedByPersonAliasId = model.ModifiedByPersonAliasId,
            };

            AddAttributesToViewModel( model, viewModel, currentPerson, loadAttributes );
            ApplyAdditionalPropertiesAndSecurityToViewModel( model, viewModel, currentPerson, loadAttributes );
            return viewModel;
        }
    }


    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class DefinedValueExtensionMethods
    {
        /// <summary>
        /// Clones this DefinedValue object to a new DefinedValue object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static DefinedValue Clone( this DefinedValue source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as DefinedValue;
            }
            else
            {
                var target = new DefinedValue();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Clones this DefinedValue object to a new DefinedValue object with default values for the properties in the Entity and Model base classes.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static DefinedValue CloneWithoutIdentity( this DefinedValue source )
        {
            var target = new DefinedValue();
            target.CopyPropertiesFrom( source );

            target.Id = 0;
            target.Guid = Guid.NewGuid();
            target.ForeignKey = null;
            target.ForeignId = null;
            target.ForeignGuid = null;
            target.CreatedByPersonAliasId = null;
            target.CreatedDateTime = RockDateTime.Now;
            target.ModifiedByPersonAliasId = null;
            target.ModifiedDateTime = RockDateTime.Now;

            return target;
        }

        /// <summary>
        /// Copies the properties from another DefinedValue object to this DefinedValue object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this DefinedValue target, DefinedValue source )
        {
            target.Id = source.Id;
            target.DefinedTypeId = source.DefinedTypeId;
            target.Description = source.Description;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.IsActive = source.IsActive;
            target.IsSystem = source.IsSystem;
            target.Order = source.Order;
            target.Value = source.Value;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }

        /// <summary>
        /// Creates a view model from this entity
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson" >The currentPerson.</param>
        /// <param name="loadAttributes" >Load attributes?</param>
        public static Rock.ViewModel.DefinedValueViewModel ToViewModel( this DefinedValue model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new DefinedValueViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}