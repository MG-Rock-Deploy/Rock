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
    /// Step View Model
    /// </summary>
    public partial class StepBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the Id of the Rock.Model.Campus associated with this step.
        /// </summary>
        public int? CampusId { get; set; }

        /// <summary>
        /// Gets or sets the System.DateTime associated with the completion of this step.
        /// </summary>
        public DateTime? CompletedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the System.DateTime associated with the end of this step.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Rock.Model.PersonAlias that identifies the Person associated with taking this step. This property is required.
        /// </summary>
        public int PersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the System.DateTime associated with the start of this step.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Rock.Model.StepProgramCompletion to which this step belongs.
        /// </summary>
        public int? StepProgramCompletionId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Rock.Model.StepStatus to which this step belongs.
        /// </summary>
        public int? StepStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Rock.Model.StepType to which this step belongs. This property is required.
        /// </summary>
        public int StepTypeId { get; set; }

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