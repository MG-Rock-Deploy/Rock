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

import { PublicAttributeBag } from "@Obsidian/ViewModels/Utility/publicAttributeBag";

/** GroupLocationHistorical View Model */
export type GroupLocationHistoricalBag = {
    /**
     * Gets or sets a value indicating whether [current row indicator].
     * This will be True if this represents the same values as the current tracked record for this
     */
    currentRowIndicator: boolean;

    /**
     * Gets or sets the effective date.
     * This is the starting date that the tracked record had the values reflected in this record
     */
    effectiveDateTime?: string | null;

    /**
     * Gets or sets the expire date time
     * This is the last date that the tracked record had the values reflected in this record
     * For example, if a tracked record's Name property changed on '2016-07-14', the ExpireDate of the previously current record will be '2016-07-13', and the EffectiveDate of the current record will be '2016-07-14'
     * If this is most current record, the ExpireDate will be '9999-01-01'
     */
    expireDateTime?: string | null;

    /** Gets or sets the Rock.Model.Group id for this group's location at this point in history */
    groupId: number;

    /** Gets or sets the group location identifier that this is a Historical snapshot for */
    groupLocationId?: number | null;

    /** Gets or sets the group's location type name at this point in history (Group.GroupLocation.GroupLocationTypeValue.Value) */
    groupLocationTypeName?: string | null;

    /** Gets or sets the group location type value identifier for this group location at this point in history */
    groupLocationTypeValueId?: number | null;

    /** Gets or sets the Rock.Model.Location id of this group's location at this point in history */
    locationId: number;

    /** Gets or sets the Location's ModifiedDateTime. This is used internally to detect if the group's location has changed */
    locationModifiedDateTime?: string | null;

    /** Gets or sets the Rock.Model.Location name of this group's location at this point in history (Group.GroupLocation.Location.ToString()) */
    locationName?: string | null;

    /** Gets or sets the created date time. */
    createdDateTime?: string | null;

    /** Gets or sets the modified date time. */
    modifiedDateTime?: string | null;

    /** Gets or sets the created by person alias identifier. */
    createdByPersonAliasId?: number | null;

    /** Gets or sets the modified by person alias identifier. */
    modifiedByPersonAliasId?: number | null;

    /** Gets or sets the identifier key of this entity. */
    idKey?: string | null;

    /** Gets or sets the attributes. */
    attributes?: Record<string, PublicAttributeBag> | null;

    /** Gets or sets the attribute values. */
    attributeValues?: Record<string, string> | null;
};
