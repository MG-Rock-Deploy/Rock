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
//

using System;
using System.Linq;

using Rock.ViewModels.Utility;

namespace Rock.ViewModels.Entities
{
    /// <summary>
    /// FinancialAccount View Model
    /// </summary>
    public partial class FinancialAccountBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the DefinedValueId of the Rock.Model.DefinedValue that represents the FinancialAccountType for this FinancialAccount.
        /// </summary>
        /// <value>
        /// A System.Int32 representing DefinedValueId of the FinancialAccountType's Rock.Model.DefinedValue for this FinancialAccount.
        /// </value>
        public int? AccountTypeValueId { get; set; }

        /// <summary>
        /// Gets or sets the CampusId of the Rock.Model.Campus that this FinancialAccount is associated with. If this FinancialAccount is not
        /// associated with a Rock.Model.Campus this property will be null.
        /// </summary>
        /// <value>
        /// A System.Int32 representing the CampusId of the Rock.Model.Campus that the FinancialAccount is associated with.
        /// </value>
        public int? CampusId { get; set; }

        /// <summary>
        /// Gets or sets the user defined description of the FinancialAccount.
        /// </summary>
        /// <value>
        /// A System.String representing the user defined description of the FinancialAccount.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the closing/end date for this FinancialAccount. This is the last day that transactions can be posted to this account. If there is not a end date
        /// for this account, transactions can be posted for an indefinite period of time.  Ongoing FinancialAccounts will not have an end date.
        /// </summary>
        /// <value>
        /// A System.DateTime representing the closing/end date for this FinancialAccounts. Transactions can be posted to this account until this date.  If this is 
        /// an ongoing account, this property will be null.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the General Ledger account code for this FinancialAccount.
        /// </summary>
        /// <value>
        /// A System.String representing the General Ledger account code for this FinancialAccount.
        /// </value>
        public string GlCode { get; set; }

        /// <summary>
        /// Gets or sets the Image Id that can be used when displaying this Financial Account
        /// </summary>
        /// <value>
        /// The image binary file identifier.
        /// </value>
        public int? ImageBinaryFileId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this FinancialAccount is active.
        /// </summary>
        /// <value>
        ///  A System.Boolean that is true if this FinancialAccount is active, otherwise false.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this FinancialAccount is public.
        /// </summary>
        /// <value>
        ///  A System.Boolean that is true if this FinancialAccount is public, otherwise false.
        /// </value>
        public bool? IsPublic { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if transactions posted to this FinancialAccount are tax-deductible.
        /// </summary>
        /// <value>
        /// A System.Boolean that is true if transactions posted to this FinancialAccount are tax-deductible; otherwise false.
        /// </value>
        public bool IsTaxDeductible { get; set; }

        /// <summary>
        /// Gets or sets the (internal) Name of the FinancialAccount. This property is required.
        /// </summary>
        /// <value>
        /// A System.String representing the (internal) name of the FinancialAccount.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sort and display order of the FinancialAccount.  This is an ascending order, so the lower the value the higher the sort priority.
        /// </summary>
        /// <value>
        /// A System.Int32 representing the sort order of the FinancialAccount.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the FinancialAccountId of the parent FinancialAccount to this FinancialAccount. If this
        /// FinancialAccount does not have a parent, this property will be null.
        /// </summary>
        /// <value>
        /// A System.Int32 representing the FinancialAccountId of the parent FinancialAccount to this FinancialAccount. 
        /// This property will be null if the FinancialAccount does not have a parent.
        /// </value>
        public int? ParentAccountId { get; set; }

        /// <summary>
        /// Gets or sets the user defined public description of the FinancialAccount.
        /// </summary>
        /// <value>
        /// A System.String representing the user defined public description of the FinancialAccount.
        /// </value>
        public string PublicDescription { get; set; }

        /// <summary>
        /// Gets or sets the public name of the Financial Account.
        /// </summary>
        /// <value>
        /// A System.String that represents the public name of the FinancialAccount.
        /// </value>
        public string PublicName { get; set; }

        /// <summary>
        /// Gets or sets the opening date for this FinancialAccount. This is the first date that transactions can be posted to this account. 
        /// If there isn't a start date for this account, transactions can be posted as soon as the account is created until the Rock.Model.FinancialAccount.EndDate (if applicable).
        /// </summary>
        /// <value>
        /// A System.DateTime representing the first day that transactions can posted to this account. If there is no start date, this property will be null.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the URL which could be used to generate a link to a 'More Info' page
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the modified date time.
        /// </summary>
        /// <value>
        /// The modified date time.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created by person alias identifier.
        /// </summary>
        /// <value>
        /// The created by person alias identifier.
        /// </value>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the modified by person alias identifier.
        /// </summary>
        /// <value>
        /// The modified by person alias identifier.
        /// </value>
        public int? ModifiedByPersonAliasId { get; set; }

    }
}