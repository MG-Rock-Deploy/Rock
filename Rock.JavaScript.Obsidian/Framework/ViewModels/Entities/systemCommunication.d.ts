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

import { IEntity } from "../entity";

export type SystemCommunication = IEntity & {
    bcc?: string | null;
    body?: string | null;
    categoryId?: number | null;
    cc?: string | null;
    cssInliningEnabled?: boolean;
    from?: string | null;
    fromName?: string | null;
    idKey?: string | null;
    isActive?: boolean | null;
    isSystem?: boolean;
    lavaFieldsJson?: string | null;
    pushData?: string | null;
    pushImageBinaryFileId?: number | null;
    pushMessage?: string | null;
    pushOpenAction?: number | null;
    pushOpenMessage?: string | null;
    pushSound?: string | null;
    pushTitle?: string | null;
    sMSFromDefinedValueId?: number | null;
    sMSMessage?: string | null;
    subject?: string | null;
    title?: string | null;
    to?: string | null;
    createdDateTime?: string | null;
    modifiedDateTime?: string | null;
    createdByPersonAliasId?: number | null;
    modifiedByPersonAliasId?: number | null;
};