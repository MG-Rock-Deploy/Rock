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

import { Ref } from "vue";
import { Guid } from "..";
import { HttpBodyData, HttpResult } from "./http";

/** A security grant generated by the server. */
export type SecurityGrant = {
    /** The token string that provides additional security access. */
    token: Ref<string | null>;

    /** Manually updates the token with a new one received from the server. */
    updateToken: (newToken: string | null | undefined) => void;
};

/** The data contained in a block event. */
export type BlockEvent<T = Record<string, unknown> | undefined> = {
    /** The unique identifier of the block that dispatched the event. */
    guid: Guid,

    /** The custom data that was attached to the event. */
    data: T
};

/** A function that will invoke a block action. */
export type InvokeBlockActionFunc = <T>(actionName: string, data?: HttpBodyData) => Promise<HttpResult<T>>;