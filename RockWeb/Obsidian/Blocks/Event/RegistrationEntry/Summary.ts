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

import { defineComponent, inject } from 'vue';
import GatewayControl, { GatewayControlModel } from '../../../Controls/GatewayControl';
import { InvokeBlockActionFunc } from '../../../Controls/RockBlock';
import RockForm from '../../../Controls/RockForm';
import RockValidation from '../../../Controls/RockValidation';
import Alert from '../../../Elements/Alert';
import CheckBox from '../../../Elements/CheckBox';
import CurrencyBox from '../../../Elements/CurrencyBox';
import EmailBox from '../../../Elements/EmailBox';
import JavaScriptAnchor from '../../../Elements/JavaScriptAnchor';
import RockButton from '../../../Elements/RockButton';
import StaticFormControl from '../../../Elements/StaticFormControl';
import TextBox from '../../../Elements/TextBox';
import { ruleArrayToString } from '../../../Rules/Index';
import { asFormattedString } from '../../../Services/Number';
import { Guid } from '../../../Util/Guid';
import Person from '../../../ViewModels/CodeGenerated/PersonViewModel';
import { getRegistrantBasicInfo, RegistrantBasicInfo, RegistrationEntryState } from '../RegistrationEntry';
import { RegistrationEntryBlockArgs } from './RegistrationEntryBlockArgs';
import { RegistrantInfo, RegistrarInfo, RegistrarOption, RegistrationEntryBlockSuccessViewModel, RegistrationEntryBlockViewModel } from './RegistrationEntryBlockViewModel';

type CheckDiscountCodeResult = {
    DiscountCode: string;
    UsagesRemaining: number | null;
    DiscountAmount: number;
    DiscountPercentage: number;
};

type LineItem = {
    Key: Guid;
    IsFee: boolean;
    Description: string;
    Amount: number;
    AmountFormatted: string;
    DiscountedAmount: number;
    DiscountedAmountFormatted: string;
    DiscountHelp: string;
};

export default defineComponent( {
    name: 'Event.RegistrationEntry.Summary',
    components: {
        RockButton,
        TextBox,
        CheckBox,
        EmailBox,
        RockForm,
        Alert,
        GatewayControl,
        RockValidation,
        JavaScriptAnchor,
        CurrencyBox,
        StaticFormControl
    },
    setup ()
    {
        return {
            getRegistrationEntryBlockArgs: inject( 'getRegistrationEntryBlockArgs' ) as () => RegistrationEntryBlockArgs,
            invokeBlockAction: inject( 'invokeBlockAction' ) as InvokeBlockActionFunc,
            registrationEntryState: inject( 'registrationEntryState' ) as RegistrationEntryState
        };
    },
    data ()
    {
        return {
            /** Is there an AJAX call in-flight? */
            loading: false,

            /** The bound value to the discount code input */
            discountCodeInput: '',

            /** A warning message about the discount code that is a result of a failed AJAX call */
            discountCodeWarningMessage: '',

            /** The dollar amount to be discounted because of the discount code entered. */
            discountAmount: 0,

            /** The percent of the total to be discounted because of the discount code entered. */
            discountPercent: 0,

            /** Should the gateway control submit to the gateway to create a token? */
            doGatewayControlSubmit: false,

            /** Gateway indicated error */
            gatewayErrorMessage: '',

            /** Gateway indicated validation issues */
            gatewayValidationFields: {} as Record<string, string>,

            /** An error message received from a bad submission */
            submitErrorMessage: '',

            /** The amount already paid in the past (of an existing registration) */
            previouslyPaid: 0,

            /** Should the registrar panel be shown */
            isRegistrarPanelShown: true
        };
    },
    computed: {
        /** The success message displayed once a discount code has been applied */
        discountCodeSuccessMessage (): string
        {
            const discountAmount = this.viewModel.Session?.DiscountAmount || this.discountAmount;
            const discountPercent = this.viewModel.Session?.DiscountPercentage || this.discountPercent;

            if ( !discountPercent && !discountAmount )
            {
                return '';
            }

            const discountText = discountPercent ?
                `${asFormattedString( discountPercent * 100, 0 )}%` :
                `$${asFormattedString( discountAmount, 2 )}`;

            return `Your ${discountText} discount code for all registrants was successfully applied.`;
        },

        /** Should the discount panel be shown? */
        isDiscountPanelVisible (): boolean
        {
            if ( !this.viewModel.HasDiscountsAvailable || !this.maxAmountCanBePaid )
            {
                return false;
            }

            return true;
        },

        /** Is the registrar option set to UseLoggedInPerson */
        useLoggedInPersonForRegistrar (): boolean
        {
            return ( !!this.currentPerson ) && this.viewModel.RegistrarOption === RegistrarOption.UseLoggedInPerson;
        },

        /** The settings for the gateway (MyWell, etc) control */
        gatewayControlModel (): GatewayControlModel
        {
            return this.viewModel.GatewayControl;
        },

        /** The person that is currently authenticated */
        currentPerson (): Person | null
        {
            return this.$store.state.currentPerson;
        },

        /** The person entering the registration information. This object is part of the registration state. */
        registrar (): RegistrarInfo
        {
            return this.registrationEntryState.Registrar;
        },

        /** The first registrant entered into the registration. */
        firstRegistrant (): RegistrantInfo
        {
            return this.registrationEntryState.Registrants[ 0 ];
        },

        /** This is the data sent from the C# code behind when the block initialized. */
        viewModel (): RegistrationEntryBlockViewModel
        {
            return this.registrationEntryState.ViewModel;
        },

        /** Should the checkbox allowing the registrar to choose to update their email address be shown? */
        doShowUpdateEmailOption (): boolean
        {
            return !this.viewModel.ForceEmailUpdate && !!this.currentPerson?.Email;
        },

        /** Should the discount column in the fee table be shown? */
        showDiscountCol (): boolean
        {
            return this.discountPercent > 0 || this.discountAmount > 0;
        },

        /** Info about the registrants made available by .FirstName instead of by field guid */
        registrantInfos (): RegistrantBasicInfo[]
        {
            return this.registrationEntryState.Registrants.map( r => getRegistrantBasicInfo( r, this.viewModel.RegistrantForms ) );
        },

        /** The fee line items that will be displayed in the summary */
        lineItems (): LineItem[]
        {
            const lineItems: LineItem[] = [];

            for ( const registrant of this.registrationEntryState.Registrants )
            {
                let total = this.viewModel.Cost;
                let discountedTotal = total;
                let discountRemaining = 0;

                if ( this.discountAmount && total < this.discountAmount )
                {
                    discountRemaining = this.discountAmount - total;
                    discountedTotal = 0;
                }
                else if ( this.discountAmount )
                {
                    discountedTotal = total - this.discountAmount;
                }
                else if ( this.discountPercent )
                {
                    const discount = this.discountPercent >= 1 ?
                        this.total :
                        this.discountPercent <= 0 ?
                            0 :
                            ( total * this.discountPercent );

                    discountedTotal = total - discount;
                }

                const info = this.registrantInfos.find( r => r.Guid === registrant.Guid )!;
                const name = registrant.IsOnWaitList ?
                    `${info.FirstName} ${info.LastName} (Waiting List)` :
                    `${info.FirstName} ${info.LastName}`;

                if ( registrant.IsOnWaitList )
                {
                    total = 0;
                    discountedTotal = 0;
                }

                lineItems.push( {
                    Key: registrant.Guid,
                    IsFee: false,
                    Description: name,
                    Amount: total,
                    AmountFormatted: asFormattedString( total ),
                    DiscountedAmount: discountedTotal,
                    DiscountedAmountFormatted: asFormattedString( discountedTotal ),
                    DiscountHelp: ''
                } );

                // Don't show fees if on the waitlist
                if ( registrant.IsOnWaitList )
                {
                    continue;
                }

                for ( const fee of this.viewModel.Fees )
                {
                    for ( const feeItem of fee.Items )
                    {
                        const qty = registrant.FeeItemQuantities[ feeItem.Guid ];

                        if ( !qty )
                        {
                            continue;
                        }

                        const itemTotal = qty * feeItem.Cost;
                        let itemDiscountedTotal = itemTotal;

                        if ( fee.DiscountApplies )
                        {
                            if ( itemTotal < discountRemaining )
                            {
                                discountRemaining -= itemTotal;
                                itemDiscountedTotal = 0;
                            }
                            else if ( discountRemaining )
                            {
                                itemDiscountedTotal -= discountRemaining;
                                discountRemaining = 0;
                            }
                            else if ( this.discountPercent )
                            {
                                const discount = this.discountPercent >= 1 ?
                                    itemTotal :
                                    this.discountPercent <= 0 ?
                                        0 :
                                        ( itemTotal * this.discountPercent );

                                itemDiscountedTotal = itemTotal - discount;
                            }
                        }

                        lineItems.push( {
                            Key: `${registrant.Guid}-${feeItem.Guid}`,
                            IsFee: true,
                            Description: `${fee.Name}-${feeItem.Name} (${qty} @ $${asFormattedString( feeItem.Cost )})`,
                            Amount: itemTotal,
                            AmountFormatted: asFormattedString( itemTotal ),
                            DiscountedAmount: itemDiscountedTotal,
                            DiscountedAmountFormatted: asFormattedString( itemDiscountedTotal ),
                            DiscountHelp: fee.DiscountApplies ? '' : 'This item is not eligible for the discount.'
                        } );
                    }
                }
            }

            return lineItems;
        },

        /** The total amount of the registration before discounts */
        total (): number
        {
            let total = 0;

            for ( const lineItem of this.lineItems )
            {
                total += lineItem.Amount;
            }

            return total;
        },

        /** The total amount of the registration after discounts */
        discountedTotal (): number
        {
            let total = 0;

            for ( const lineItem of this.lineItems )
            {
                total += lineItem.DiscountedAmount;
            }

            return total;
        },

        /** The total before discounts as a formatted string */
        totalFormatted (): string
        {
            return `$${asFormattedString( this.total )}`;
        },

        /** The total after discounts as a formatted string */
        discountedTotalFormatted (): string
        {
            return `$${asFormattedString( this.discountedTotal )}`;
        },

        /** Is there a user selectable amount to pay today (as opposed to paying in full) */
        showAmountDueToday (): boolean
        {
            return this.viewModel.AmountDueToday !== null;
        },

        /** The amount due today */
        amountDueToday (): number
        {
            const amountDue = ( ( this.viewModel.AmountDueToday || 0 ) * this.registrationEntryState.Registrants.length ) - this.previouslyPaid;

            if ( amountDue > 0 )
            {
                return amountDue;
            }

            return 0;
        },

        /** The amount due today formatted as currency */
        amountDueTodayFormatted (): string
        {
            return `$${asFormattedString( this.amountDueToday )}`;
        },

        /** The maximum amount that can be paid */
        maxAmountCanBePaid (): number
        {
            const balance = this.discountedTotal - this.previouslyPaid;

            if ( balance > 0 )
            {
                return balance;
            }

            return 0;
        },

        /** The total after discounts and previous payments as a formatted string */
        maxAmountCanBePaidFormatted (): string
        {
            return `$${asFormattedString( this.maxAmountCanBePaid )}`;
        },

        /** The vee-validate rules for the amount to pay today */
        amountToPayTodayRules (): string
        {
            var rules: string[] = [ 'required' ];
            const registrantCount = this.registrationEntryState.Registrants.length;
            let min = this.previouslyPaid ? 0 : ( ( this.viewModel.AmountDueToday || 0 ) * registrantCount );
            const max = this.maxAmountCanBePaid;

            if ( min > max )
            {
                min = max;
            }

            rules.push( `gte:${min}` );
            rules.push( `lte:${max}` );
            return ruleArrayToString( rules );
        },

        /** After previous payments and the payment for today, how much will remain of the total? */
        amountRemaining (): number
        {
            const actual = this.discountedTotal - this.registrationEntryState.AmountToPayToday - this.previouslyPaid;
            const bounded = actual < 0 ? 0 : actual > this.discountedTotal ? this.discountedTotal : actual;
            return bounded;
        },

        /** After the initial payment, how much will remain of the total? Formatted as currency. */
        amountRemainingFormatted (): string
        {
            return `$${asFormattedString( this.amountRemaining )}`;
        },

        /** The registrant term - plural if there are more than 1 */
        registrantTerm (): string
        {
            return this.registrantInfos.length === 1 ? this.viewModel.RegistrantTerm : this.viewModel.PluralRegistrantTerm;
        },

        /** The name of this registration instance */
        instanceName (): string
        {
            return this.viewModel.InstanceName;
        },

        previouslyPaidFormatted (): string
        {
            return `$${asFormattedString( this.previouslyPaid )}`;
        },

        /** The text to be displayed on the "Finish" button */
        finishButtonText (): string
        {
            return ( this.viewModel.IsRedirectGateway && this.registrationEntryState.AmountToPayToday ) ? 'Pay' : 'Finish';
        }
    },
    methods: {
        /** User clicked the "previous" button */
        onPrevious ()
        {
            this.$emit( 'previous' );
        },

        /** User clicked the "finish" button */
        async onNext ()
        {
            this.loading = true;

            // If there is a cost, then the gateway will need to be used to pay
            if ( this.maxAmountCanBePaid )
            {
                // If this is a redirect gateway, then persist and redirect now
                if ( this.viewModel.IsRedirectGateway )
                {
                    const redirectUrl = await this.getPaymentRedirect();

                    if ( redirectUrl )
                    {
                        location.href = redirectUrl;
                    }
                }
                else
                {
                    // Otherwise, this is a traditional gateway
                    this.gatewayErrorMessage = '';
                    this.gatewayValidationFields = {};
                    this.doGatewayControlSubmit = true;
                }
            }
            else
            {
                const success = await this.submit();
                this.loading = false;

                if ( success )
                {
                    this.$emit( 'next' );
                }
            }
        },

        /** Send a user input discount code to the server so the server can check and send back
         *  the discount amount. */
        async tryDiscountCode ()
        {
            this.loading = true;

            try
            {
                const result = await this.invokeBlockAction<CheckDiscountCodeResult>( 'CheckDiscountCode', {
                    code: this.discountCodeInput
                } );

                if ( result.isError || !result.data )
                {
                    this.discountCodeWarningMessage = `'${this.discountCodeInput}' is not a valid Discount Code.`;
                }
                else
                {
                    this.discountCodeWarningMessage = '';
                    this.discountAmount = result.data.DiscountAmount;
                    this.discountPercent = result.data.DiscountPercentage;
                    this.registrationEntryState.DiscountCode = result.data.DiscountCode;
                }
            }
            finally
            {
                this.loading = false;
            }
        },

        /** Prefill in the registrar form fields based on the admin's settings */
        prefillRegistrar ()
        {
            this.isRegistrarPanelShown = true;

            // If the option is to prompt or use the current person, prefill the current person if available
            if ( this.currentPerson &&
                ( this.viewModel.RegistrarOption === RegistrarOption.UseLoggedInPerson || this.viewModel.RegistrarOption === RegistrarOption.PromptForRegistrar ) )
            {
                this.registrar.NickName = this.currentPerson.NickName || this.currentPerson.FirstName || '';
                this.registrar.LastName = this.currentPerson.LastName || '';
                this.registrar.Email = this.currentPerson.Email || '';
                return;
            }

            if ( this.viewModel.RegistrarOption === RegistrarOption.PromptForRegistrar )
            {
                return;
            }

            // If prefill or first-registrant, then the first registrants info is used (as least as a starting point)
            if ( this.viewModel.RegistrarOption === RegistrarOption.PrefillFirstRegistrant || this.viewModel.RegistrarOption === RegistrarOption.UseFirstRegistrant )
            {
                const firstRegistrantInfo = getRegistrantBasicInfo( this.firstRegistrant, this.viewModel.RegistrantForms );
                this.registrar.NickName = firstRegistrantInfo.FirstName;
                this.registrar.LastName = firstRegistrantInfo.LastName;
                this.registrar.Email = firstRegistrantInfo.Email;

                const hasAllInfo = ( !!this.registrar.NickName ) && ( !!this.registrar.LastName ) && ( !!this.registrar.Email );

                if ( hasAllInfo && this.viewModel.RegistrarOption === RegistrarOption.UseFirstRegistrant )
                {
                    this.isRegistrarPanelShown = false;
                }

                return;
            }
        },

        /**
         * The gateway indicated success and returned a token
         * @param token
         */
        async onGatewayControlSuccess ( token: string )
        {
            this.registrationEntryState.GatewayToken = token;
            const success = await this.submit();
            this.loading = false;

            if ( success )
            {
                this.$emit( 'next' );
            }
        },

        /** The gateway was requested by the user to reset. The token should be cleared */
        async onGatewayControlReset ()
        {
            this.registrationEntryState.GatewayToken = '';
            this.doGatewayControlSubmit = false;
        },

        /**
         * The gateway indicated an error
         * @param message
         */
        onGatewayControlError ( message: string )
        {
            this.doGatewayControlSubmit = false;
            this.loading = false;
            this.gatewayErrorMessage = message;
        },

        /**
         * The gateway wants the user to fix some fields
         * @param invalidFields
         */
        onGatewayControlValidation ( invalidFields: Record<string, string> )
        {
            this.doGatewayControlSubmit = false;
            this.loading = false;
            this.gatewayValidationFields = invalidFields;
        },

        /** Submit the registration to the server */
        async submit (): Promise<boolean>
        {
            const result = await this.invokeBlockAction<RegistrationEntryBlockSuccessViewModel>( 'SubmitRegistration', {
                args: this.getRegistrationEntryBlockArgs()
            } );

            if ( result.isError || !result.data )
            {
                this.submitErrorMessage = result.errorMessage || 'Unknown error';
            }
            else
            {
                this.registrationEntryState.SuccessViewModel = result.data;
            }

            return result.isSuccess;
        },

        /** Persist the args to the server so the user can be redirected for payment. Returns the redirect URL. */
        async getPaymentRedirect (): Promise<string>
        {
            const result = await this.invokeBlockAction<string>( 'GetPaymentRedirect', {
                args: this.getRegistrationEntryBlockArgs()
            } );

            if ( result.isError || !result.data )
            {
                this.submitErrorMessage = result.errorMessage || 'Unknown error';
            }

            return result.data || '';
        }
    },
    created ()
    {
        if ( this.viewModel.Session )
        {
            this.registrationEntryState.AmountToPayToday = this.viewModel.Session.AmountToPayNow;
            this.discountCodeInput = this.viewModel.Session.DiscountCode;
            this.discountAmount = this.viewModel.Session.DiscountAmount;
            this.discountPercent = this.viewModel.Session.DiscountPercentage;
            this.previouslyPaid = this.viewModel.Session.PreviouslyPaid;
        }
        else if ( this.viewModel.InitialAmountToPay !== null )
        {
            this.registrationEntryState.AmountToPayToday = this.viewModel.InitialAmountToPay * this.registrationEntryState.Registrants.length;
        }
        else
        {
            this.registrationEntryState.AmountToPayToday = this.discountAmount;
        }
    },
    watch: {
        currentPerson: {
            immediate: true,
            handler ()
            {
                this.prefillRegistrar();
            }
        },

        /** When the discount code changes, then adjust the amount to pay */
        'registrationEntryState.DiscountCode' ()
        {
            this.registrationEntryState.AmountToPayToday = this.maxAmountCanBePaid;
        }
    },
    template: `
<div class="registrationentry-summary">
    <RockForm @submit="onNext">

        <div v-if="isRegistrarPanelShown" class="well">
            <h4>This Registration Was Completed By</h4>
            <template v-if="useLoggedInPersonForRegistrar">
                <div class="row">
                    <div class="col-md-6">
                        <StaticFormControl label="First Name" v-model="registrar.NickName" />
                    </div>
                    <div class="col-md-6">
                        <StaticFormControl label="Last Name" v-model="registrar.LastName" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <StaticFormControl label="Email" v-model="registrar.Email" />
                    </div>
                </div>
            </template>
            <template v-else>
                <div class="row">
                    <div class="col-md-6">
                        <TextBox label="First Name" rules="required" v-model="registrar.NickName" />
                    </div>
                    <div class="col-md-6">
                        <TextBox label="Last Name" rules="required" v-model="registrar.LastName" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <EmailBox label="Send Confirmation Emails To" rules="required" v-model="registrar.Email" />
                        <CheckBox v-if="doShowUpdateEmailOption" label="Should Your Account Be Updated To Use This Email Address?" v-model="registrar.UpdateEmail" />
                    </div>
                </div>
            </template>
        </div>

        <div v-if="total">
            <h4>Payment Summary</h4>
            <Alert v-if="discountCodeWarningMessage" alertType="warning">{{discountCodeWarningMessage}}</Alert>
            <Alert v-if="discountCodeSuccessMessage" alertType="success">{{discountCodeSuccessMessage}}</Alert>
            <div v-if="isDiscountPanelVisible || discountCodeInput" class="clearfix">
                <div class="form-group pull-right">
                    <label class="control-label">Discount Code</label>
                    <div class="input-group">
                        <input type="text" :disabled="loading || !!discountCodeSuccessMessage" class="form-control input-width-md input-sm" v-model="discountCodeInput" />
                        <RockButton v-if="!discountCodeSuccessMessage" btnSize="sm" :isLoading="loading" class="margin-l-sm" @click="tryDiscountCode">
                            Apply
                        </RockButton>
                    </div>
                </div>
            </div>
            <div class="fee-table">
                <div class="row hidden-xs fee-header">
                    <div class="col-sm-6">
                        <strong>Description</strong>
                    </div>
                    <div v-if="showDiscountCol" class="col-sm-3 fee-value">
                        <strong>Discounted Amount</strong>
                    </div>
                    <div class="col-sm-3 fee-value">
                        <strong>Amount</strong>
                    </div>
                </div>
                <div v-for="lineItem in lineItems" :key="lineItem.Key" class="row" :class="lineItem.IsFee ? 'fee-row-fee' : 'fee-row-cost'">
                    <div class="col-sm-6 fee-caption">
                        {{lineItem.Description}}
                    </div>
                    <div v-if="showDiscountCol" class="col-sm-3 fee-value">
                        <JavaScriptAnchor v-if="lineItem.DiscountHelp" class="help" :title="lineItem.DiscountHelp">
                            <i class="fa fa-info-circle"></i>
                        </JavaScriptAnchor>
                        <span class="visible-xs-inline">Discounted Amount:</span>
                        $ {{lineItem.DiscountedAmountFormatted}}
                    </div>
                    <div class="col-sm-3 fee-value">
                        <span class="visible-xs-inline">Amount:</span>
                        $ {{lineItem.AmountFormatted}}
                    </div>
                </div>
                <div class="row fee-row-total">
                    <div class="col-sm-6 fee-caption">
                        Total
                    </div>
                    <div v-if="showDiscountCol" class="col-sm-3 fee-value">
                        <span class="visible-xs-inline">Discounted Amount:</span>
                        {{discountedTotalFormatted}}
                    </div>
                    <div class="col-sm-3 fee-value">
                        <span class="visible-xs-inline">Amount:</span>
                        {{totalFormatted}}
                    </div>
                </div>
            </div>

            <div class="row fee-totals">
                <div class="col-sm-offset-8 col-sm-4 fee-totals-options">
                    <div class="form-group static-control">
                        <label class="control-label">Total Cost</label>
                        <div class="control-wrapper">
                            <div class="form-control-static">
                                {{discountedTotalFormatted}}
                            </div>
                        </div>
                    </div>
                    <div v-if="previouslyPaid" class="form-group static-control">
                        <label class="control-label">Previously Paid</label>
                        <div class="control-wrapper">
                            <div class="form-control-static">
                                {{previouslyPaidFormatted}}
                            </div>
                        </div>
                    </div>
                    <template v-if="showAmountDueToday && maxAmountCanBePaid">
                        <div class="form-group static-control">
                            <label class="control-label">Minimum Due Today</label>
                            <div class="control-wrapper">
                                <div class="form-control-static">
                                    {{amountDueTodayFormatted}}
                                </div>
                            </div>
                        </div>
                        <CurrencyBox label="Amount To Pay Today" :rules="amountToPayTodayRules" v-model="registrationEntryState.AmountToPayToday" class="form-right" inputGroupClasses="input-width-md amount-to-pay" />
                        <div class="form-group static-control">
                            <label class="control-label">Amount Remaining</label>
                            <div class="control-wrapper">
                                <div class="form-control-static">
                                    {{amountRemainingFormatted}}
                                </div>
                            </div>
                        </div>
                    </template>
                    <div v-else class="form-group static-control">
                        <label class="control-label">Amount Due</label>
                        <div class="control-wrapper">
                            <div class="form-control-static">
                                {{maxAmountCanBePaidFormatted}}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div v-if="gatewayControlModel && total && maxAmountCanBePaid" class="well">
            <h4>Payment Method</h4>
            <Alert v-if="gatewayErrorMessage" alertType="danger">{{gatewayErrorMessage}}</Alert>
            <RockValidation :errors="gatewayValidationFields" />
            <div class="hosted-payment-control">
                <GatewayControl
                    :gatewayControlModel="gatewayControlModel"
                    :submit="doGatewayControlSubmit"
                    @success="onGatewayControlSuccess"
                    @reset="onGatewayControlReset"
                    @error="onGatewayControlError"
                    @validation="onGatewayControlValidation" />
            </div>
        </div>

        <div v-if="!total" class="margin-b-md">
            <p>The following {{registrantTerm}} will be registered for {{instanceName}}:</p>
            <ul>
                <li v-for="r in registrantInfos" :key="r.Guid">
                    <strong>{{r.FirstName}} {{r.LastName}}</strong>
                </li>
            </ul>
        </div>

        <Alert v-if="submitErrorMessage" alertType="danger">{{submitErrorMessage}}</Alert>

        <div class="actions text-right">
            <RockButton v-if="viewModel.AllowRegistrationUpdates" class="pull-left" btnType="default" @click="onPrevious" :isLoading="loading">
                Previous
            </RockButton>
            <RockButton btnType="primary" type="submit" :isLoading="loading">
                {{finishButtonText}}
            </RockButton>
        </div>
    </RockForm>
</div>`
} );