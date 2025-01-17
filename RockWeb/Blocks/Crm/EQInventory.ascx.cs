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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace Rockweb.Blocks.Crm
{
    /// <summary>
    /// Calculates a person's EQ Inventory assessment score based on a series of question answers.
    /// </summary>
    [DisplayName( "EQ Assessment" )]
    [Category( "CRM" )]
    [Description( "Allows you to take a EQ Inventory test and saves your EQ Inventory score." )]

    #region Block Attributes

    [CodeEditorField(
        "Instructions",
        Key = AttributeKey.Instructions,
        Description = "The text (HTML) to display at the top of the instructions section.  <span class='tip tip-lava'></span> <span class='tip tip-html'></span>",
        EditorMode = CodeEditorMode.Html,
        EditorTheme = CodeEditorTheme.Rock,
        EditorHeight = 400,
        IsRequired = true,
        DefaultValue = InstructionsDefaultValue,
        Order = 0 )]

    [CodeEditorField(
        "Results Message",
        Key = AttributeKey.ResultsMessage,
        Description = "The text (HTML) to display at the top of the results section.<span class='tip tip-lava'></span><span class='tip tip-html'></span>",
        EditorMode = CodeEditorMode.Html,
        EditorTheme = CodeEditorTheme.Rock,
        EditorHeight = 400,
        IsRequired = true,
        DefaultValue = ResultsMessageDefaultValue,
        Order = 1 )]

    [TextField(
        "Set Page Title",
        Key = AttributeKey.SetPageTitle,
        Description = "The text to display as the heading.",
        IsRequired = false,
        DefaultValue = "EQ Inventory Assessment",
        Order = 2 )]

    [TextField( "Set Page Icon",
        Key = AttributeKey.SetPageIcon,
        Description = "The css class name to use for the heading icon.",
        IsRequired = false,
        DefaultValue = "fa fa-theater-masks",
        Order = 3 )]

    [IntegerField(
        "Number of Questions",
        Key = AttributeKey.NumberOfQuestions,
        Description = "The number of questions to show per page while taking the test",
        IsRequired = true,
        DefaultIntegerValue = 7,
        Order = 4 )]
    #endregion Block Attributes

    [Rock.SystemGuid.BlockTypeGuid( Rock.SystemGuid.BlockType.EQ_INVENTORY )]
    public partial class EQInventory : Rock.Web.UI.RockBlock
    {
        #region Attribute Default values

        private const string InstructionsDefaultValue = @"
<h2>Welcome to the EQ Inventory Assessment</h2>
<p>
    {{ Person.NickName }}, we encounter emotions every day: our own and those of the people around us.
    This assessment measures your developed skills in two areas: understanding your emotions and
    understanding the emotions of others.
</p>
<p>
    For best results with this assessment, picture a setting such as the workplace, at home or with
    friends, and keep that same setting in mind as you answer all the questions. Your responses may
    be different in different circumstances.
</p>
<p>
    Don’t spend too much time thinking about your answer. Usually, your first responses is your most
    natural. Since there are no right or wrong answers, just go with your instinct.
</p>";

        private const string ResultsMessageDefaultValue = @"

{%- assign chartColor = '#709AC7' -%}
{%- assign chartHeight = '100px' -%}

<h1 class='text-center'>EQ Inventory Assessment Results</h1>

<p>
    {{ Person.NickName }}, here are your emotional intelligence results. This is a snapshot
    in time and may change through intentional effort and practice. You will rank high, medium
    or low in each of the following six areas.
</p>

<h3 id='eq-SelfAwareness'>Self Awareness</h3>
<p>
    Self-Awareness is being aware of what emotions you are experiencing and why you
    are experiencing them. This skill is demonstrated in real time. In other words,
    when you are in the midst of a discussion or even a disagreement with someone else,
    ask yourself these questions:
    <ul>
        <li>Are you aware of what emotions you are experiencing?</li>
        <li>Are you aware of why you are experiencing these emotions?</li>
    </ul>
</p>

<!-- Graph -->
{[ chart type:'horizontalBar' legendshow:'false' tooltipshow:'false' chartheight:'{{chartHeight}}' xaxistype:'linearhorizontal0to100' ]}
    [[ dataitem label:'Self Awareness' value:'{{SelfAwareness}}' fillcolor:'{{chartColor}}' ]] [[ enddataitem ]]
{[ endchart ]}

<blockquote>
    Your responses to the items on the Self Awareness scale indicate the score for the
    ability to be aware of your own emotions is equal to or better than {{ SelfAwareness }}%
    of those who completed this instrument.
</blockquote>

<h3 id='eq-selfregulating'>Self-Regulating</h3>
<p>
    Self-Regulating is appropriately expressing your emotions in the context of the relationships
    around you. This doesn’t indicate suppressing emotions; rather the ability to express your
    emotions appropriately. Healthy human beings experience a full range of emotions and these are
    important for family, friends, and co-workers to understand. Self-Regulating is learning to
    tell others what you are feeling in the moment.
</p>

    {[ chart type:'horizontalBar' legendshow:'false' tooltipshow:'false' chartheight:'{{chartHeight}}' xaxistype:'linearhorizontal0to100' ]}
        [[ dataitem label:'Self Regulating' value:'{{SelfRegulating}}' fillcolor:'{{chartColor}}']] [[ enddataitem ]]
    {[ endchart ]}

<blockquote>
    Your responses to the items on the Self Regulation scale indicate the score for the
    ability to appropriately express your own emotions is equal to or better than {{ SelfRegulating }}%
    of those who completed this instrument.
</blockquote>


<h3 id='eq-othersawareness'>Others-Awareness</h3>
<p>
    Others-Awareness is being aware of what emotions others are experiencing around you and
    why they are experiencing these emotions. As with understanding your own emotions, this
    skill is knowing in real time what another person is experiencing. This skill involves
    reading cues to their emotional state through their eyes, facial expressions, body
    posture, the tone of voice and many other ways.
</p>

    {[ chart type:'horizontalBar' legendshow:'false' tooltipshow:'false' chartheight:'{{chartHeight}}' xaxistype:'linearhorizontal0to100' ]}
        [[ dataitem label:'Others Awareness' value:'{{OthersAwareness}}' fillcolor:'{{chartColor}}' ]] [[ enddataitem ]]
    {[ endchart ]}

<blockquote>
    Your responses to the items on the Others-Awareness scale indicate the score for the
    ability to be aware of others emotions is equal to or better than {{ OthersAwareness }}%
    of those who completed this instrument.
</blockquote>


<h3 id='eq-othersregulating'>Others-Regulating</h3>
<p>
    Others-Regulating is helping those around you express their emotions appropriately
    in the context of your relationship with them. This skill centers on helping others
    know what emotions they are experiencing and then asking questions or giving them
    permission to freely and appropriately express their emotions in the context of
    your relationship.
</p>

    {[ chart type:'horizontalBar' legendshow:'false' tooltipshow:'false' chartheight:'{{chartHeight}}' xaxistype:'linearhorizontal0to100' ]}
        [[ dataitem label:'Others Regulating' value:'{{OthersRegulating}}' fillcolor:'{{chartColor}}' ]] [[ enddataitem ]]
    {[ endchart ]}

<blockquote>
    Your responses to the items on the Others-Regulation scale indicate the score for
    the ability to enable others to appropriately express their emotions in the context
    of your relationship is equal to or better than {{OthersRegulating}}% of those who
    completed this instrument.
</blockquote>

<h3 id='eq-problemsolving'>EQ in Problem Solving</h3>
<p>
    EQ in Problem Solving identifies how proficient you are at using emotions to solve
    problems. This skill requires first being aware of what emotions are involved in
    the problem and what is the source of those emotions. It also includes helping
    others (and yourself) express those emotions appropriate in the context of
    the situation.
</p>

    {[ chart type:'horizontalBar' legendshow:'false' tooltipshow:'false' chartheight:'{{chartHeight}}' xaxistype:'linearhorizontal0to100' ]}
        [[ dataitem label:'EQ in Problem Solving' value:'{{EQinProblemSolving}}' fillcolor:'{{chartColor}}' ]] [[ enddataitem ]]
    {[ endchart ]}

<blockquote>
    Your responses to the items on the EQ in Problem Solving scale indicate the score for
    the ability to use emotions in resolving problems is equal to or better than {{ EQinProblemSolving }}%
    of those who completed this instrument.
</blockquote>


<h3 id='eq-understress'>EQ Under Stress</h3>
<p>
    EQ Under Stress identifies how capable you are of keeping high EQ under high-stress
    moments; which is particularly challenging. This skill requires highly developed
    Self- and Others-Awareness to understand the impact of the current stress. It also
    involves being able to articulate the appropriate emotions under pressure which
    may be different from articulating them when not under stress.
</p>

    {[ chart type:'horizontalBar' legendshow:'false' tooltipshow:'false' chartheight:'{{chartHeight}}' xaxistype:'linearhorizontal0to100' ]}
        [[ dataitem label:'EQ Under Stress' value:'{{EQUnderStress}}' fillcolor:'{{chartColor}}' ]] [[ enddataitem ]]
    {[ endchart ]}

<blockquote>
    Your responses to the items on the EQ in Under Stress scale indicate the score
    for the ability to maintain EQ under significant stress is equal to or better than {{ EQUnderStress }}%
    of those who completed this instrument.
</blockquote>";

        #endregion Attribute Default values

        #region Attribute Keys
        private static class AttributeKey
        {
            public const string NumberOfQuestions = "NumberofQuestions";
            public const string Instructions = "Instructions";
            public const string SetPageTitle = "SetPageTitle";
            public const string SetPageIcon = "SetPageIcon";
            public const string ResultsMessage = "ResultsMessage";
        }

        #endregion Attribute Keys

        #region PageParameterKeys
        /// <summary>
        /// A defined list of page parameter keys used by this block.
        /// </summary>
        private static class PageParameterKey
        {
            /// <summary>
            /// The assessment identifier
            /// </summary>
            public const string AssessmentId = "AssessmentId";

            /// <summary>
            /// The ULR encoded key for a person
            /// </summary>
            public const string Person = "Person";
        }

        #endregion PageParameterKeys

        #region Fields

        private Dictionary<int, string> _negativeOption = new Dictionary<int, string>
        {
            { 5, "Never" },
            { 4, "Rarely" },
            { 3, "Sometimes" },
            { 2, "Usually" },
            { 1, "Always" }
        };

        private Dictionary<int, string> _positiveOption = new Dictionary<int, string>
        {
            { 1, "Never" },
            { 2, "Rarely" },
            { 3, "Sometimes" },
            { 4, "Usually" },
            { 5, "Always" }
        };

        // View State Keys
        private const string ASSESSMENT_STATE = "AssessmentState";
        private const string START_DATETIME = "StartDateTime";

        // View State Variables
        private List<AssessmentResponse> _assessmentResponses;

        // used for private variables
        private Person _targetPerson = null;
        private int? _assessmentId = null;
        private bool _isQuerystringPersonKey = false;

        // protected variables
        private decimal _percentComplete = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the percent complete.
        /// </summary>
        /// <value>
        /// The percent complete.
        /// </value>
        public decimal PercentComplete
        {
            get
            {
                return _percentComplete;
            }

            set
            {
                _percentComplete = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of questions
        /// </summary>
        public int QuestionCount
        {
            get { return ViewState[AttributeKey.NumberOfQuestions] as int? ?? 0; }
            set { ViewState[AttributeKey.NumberOfQuestions] = value; }
        }

        /// <summary>
        /// Gets or sets the time to take the result
        /// </summary>
        public DateTime StartDateTime
        {
            get { return ViewState[START_DATETIME] as DateTime? ?? RockDateTime.Now; }
            set { ViewState[START_DATETIME] = value; }
        }

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            _assessmentResponses = ViewState[ASSESSMENT_STATE] as List<AssessmentResponse> ?? new List<AssessmentResponse>();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            this.BlockUpdated += Block_BlockUpdated;

            SetPanelTitleAndIcon();

            _assessmentId = PageParameter( PageParameterKey.AssessmentId ).AsIntegerOrNull();
            string personKey = PageParameter( PageParameterKey.Person );

            // set the target person according to the parameter or use Current user if not provided.
            if ( personKey.IsNotNullOrWhiteSpace() )
            {
                try
                {
                    var personService = new PersonService( new RockContext() );
                    _targetPerson = personService.GetByPersonActionIdentifier( personKey, "Assessment" ) ?? personService.GetByUrlEncodedKey( personKey );
                    _isQuerystringPersonKey = true;
                }
                catch ( Exception )
                {
                    nbError.Visible = true;
                }
            }
            else if ( CurrentPerson != null )
            {
                _targetPerson = CurrentPerson;
            }

            if ( _targetPerson == null )
            {
                if ( _isQuerystringPersonKey )
                {
                    HidePanelsAndShowError( "There is an issue locating the person associated with the request." );
                }
                else
                {
                    HidePanelsAndShowError( "You must be signed in to take the assessment." );
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                ShowAssessment();
            }
            else
            {
                // Hide notification panels on every postback
                nbError.Visible = false;
            }
        }

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            ViewState[ASSESSMENT_STATE] = _assessmentResponses;

            return base.SaveViewState();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the Block control.
        /// We need to reload the page for the charts to appear.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            if ( pnlResult.Visible == true )
            {
                this.NavigateToCurrentPageReference();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnStart button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnStart_Click( object sender, EventArgs e )
        {
            StartDateTime = RockDateTime.Now;
            ShowQuestions();
        }

        /// <summary>
        /// Handles the Click event of the btnRetakeTest button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnRetakeTest_Click( object sender, EventArgs e )
        {
            hfAssessmentId.SetValue( 0 );
            ShowInstructions();
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnNext_Click( object sender, EventArgs e )
        {
            int pageNumber = hfPageNo.ValueAsInt() + 1;
            GetResponse();

            LinkButton btn = ( LinkButton ) sender;
            string commandArgument = btn.CommandArgument;

            var totalQuestion = pageNumber * QuestionCount;
            if ( ( _assessmentResponses.Count > totalQuestion && !_assessmentResponses.All( a => a.Response.HasValue ) ) || "Next".Equals( commandArgument ) )
            {
                BindRepeater( pageNumber );
            }
            else
            {
                EQInventoryService.AssessmentResults result = EQInventoryService.GetResult( _assessmentResponses.ToDictionary( a => a.Code, b => b.Response.Value ) );
                EQInventoryService.SaveAssessmentResults( _targetPerson, result );
                var rockContext = new RockContext();

                var assessmentService = new AssessmentService( rockContext );
                Assessment assessment = null;

                if ( hfAssessmentId.ValueAsInt() != 0 )
                {
                    assessment = assessmentService.Get( int.Parse( hfAssessmentId.Value ) );
                }

                if ( assessment == null )
                {
                    var assessmentType = new AssessmentTypeService( rockContext ).Get( Rock.SystemGuid.AssessmentType.EQ.AsGuid() );
                    assessment = new Assessment()
                    {
                        AssessmentTypeId = assessmentType.Id,
                        PersonAliasId = _targetPerson.PrimaryAliasId.Value
                    };
                    assessmentService.Add( assessment );
                }

                assessment.Status = AssessmentRequestStatus.Complete;
                assessment.CompletedDateTime = RockDateTime.Now;
                assessment.AssessmentResultData = new { Result = result.AssessmentData, TimeToTake = RockDateTime.Now.Subtract( StartDateTime ).TotalSeconds }.ToJson();
                rockContext.SaveChanges();

                // Since we are rendering chart.js we have to register the script or reload the page.
                if ( _assessmentId == 0 )
                {
                    var removeParams = new List<string>
                    {
                        PageParameterKey.AssessmentId
                    };

                    NavigateToCurrentPageReferenceWithRemove( removeParams );
                }
                else
                {
                    this.NavigateToCurrentPageReference();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPrevious control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnPrevious_Click( object sender, EventArgs e )
        {
            int pageNumber = hfPageNo.ValueAsInt() - 1;
            GetResponse();
            BindRepeater( pageNumber );
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rQuestions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rQuestions_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var assessmentResponseRow = e.Item.DataItem as AssessmentResponse;
            RockRadioButtonList rblQuestion = e.Item.FindControl( "rblQuestion" ) as RockRadioButtonList;

            if ( assessmentResponseRow.Code.EndsWith( "N" ) )
            {
                rblQuestion.DataSource = _negativeOption;
            }
            else
            {
                rblQuestion.DataSource = _positiveOption;
            }

            rblQuestion.DataTextField = "Value";
            rblQuestion.DataValueField = "Key";
            rblQuestion.DataBind();

            rblQuestion.Label = assessmentResponseRow.Question;

            if ( assessmentResponseRow != null && assessmentResponseRow.Response.HasValue )
            {
                rblQuestion.SetValue( assessmentResponseRow.Response );
            }
            else
            {
                rblQuestion.SetValue( string.Empty );
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Shows the assessment.
        /// A null value for _targetPerson is already handled in OnInit() so this method assumes there is a value
        /// </summary>
        private void ShowAssessment()
        {
            /*
            2020-01-09 - ETD
            This block will either show the assessment results of the most recent assessment test or give the assessment test.
            The following use cases are considered:
            1. If the assessment ID "0" was provided then create a new test for the current user. This covers user directed retakes.
            2. If the assessment ID was provided and is not "0"
                Note: The assessment results are stored on the person's attributes and are overwritten if the assessment is retaken. So past Assessments will not be loaded by this block.
                The test data is saved in the assessment table but would need to be recomputed, which may be a future feature.
                a. The assessment ID is ignored and the current person is used.
                b. If the assessment exists for the current person and is completed then show the results
                c. If the assessment exists for the current person and is pending then show the questions.
                d. If the assessment does not exist for the current person then nothing loads.
            3. If the assessment ID was not provided and the PersonKey was provided
                a. If there is only one test of the type
                    1. If the assessment is completed show the results
                    2. If the assessment is pending and the current person is the one assigned the test then show the questions.
                    3. If the assessment is pending and the current person is not the one assigned then show a message that the test has not been completed.
                b. If more than one of type
                    1. If the latest requested assessment is completed show the results.
                    2. If the latest requested assessment is pending and the current person is the one assigned then show the questions.
                    3. If the latest requested assessment is pending and the current person is not the one assigned the show the results of the last completed test.
                    4. If the latest requested assessment is pending and the current person is not the one assigned and there are no previous completed assessments then show a message that the test has not been completed.
            4. If an assessment ID or PersonKey were not provided or are not valid then show an error message
            */

            var rockContext = new RockContext();
            var assessmentType = new AssessmentTypeService( rockContext ).Get( Rock.SystemGuid.AssessmentType.EQ.AsGuid() );
            Assessment assessment = null;
            Assessment previouslyCompletedAssessment = null;

            // A "0" value indicates that the block should create a new assessment instead of looking for an existing one, so keep assessment null. e.g. a user directed re-take
            if ( _assessmentId != 0 )
            {
                var assessments = new AssessmentService( rockContext )
                .Queryable()
                .AsNoTracking()
                .Where( a => a.PersonAlias != null
                             && a.PersonAlias.PersonId == _targetPerson.Id
                             && a.AssessmentTypeId == assessmentType.Id )
                .OrderByDescending( a => a.CompletedDateTime ?? a.RequestedDateTime )
                .ToList();

                if ( _assessmentId == null && assessments.Count == 0 )
                {
                    // For this to happen the user has to have never taken the assessment, the user isn't using a link with the assessment ID, AND they are arriving at the block directly rather than through the assessment list block.
                    // So treat this as a user directed take/retake.
                    _assessmentId = 0;
                }
                else
                {
                    if ( assessments.Count > 0 )
                    {
                        // If there are any results then pick the first one. If the assesement ID was specified then the query will only return one result
                        assessment = assessments[0];
                    }
                    if ( assessments.Count > 1 )
                    {
                        // If there are more than one result then we need to pick the right one (see developer note)
                        // If the most recent assessment is "Completed" then it is already set as the assessment and we can move on. Otherwise check if there are previoulsy completed assessments.
                        if ( assessment.Status == AssessmentRequestStatus.Pending )
                        {
                            // If the most recent assessment is pending then check for a prior completed one
                            previouslyCompletedAssessment = assessments.Where( a => a.Status == AssessmentRequestStatus.Complete ).FirstOrDefault();
                        }
                    }
                }
            }

            if ( assessment == null )
            {
                // If assessment is null and _assessmentId = 0 this is user directed. If the type does not require a request then show instructions
                if ( _assessmentId == 0 && !assessmentType.RequiresRequest )
                {
                    hfAssessmentId.SetValue( 0 );
                    ShowInstructions();
                }
                else
                {
                    // If assessment is null and _assessmentId != 0 or is 0 but the type does require a request then show requires request error
                    HidePanelsAndShowError( "Sorry, this test requires a request from someone before it can be taken." );
                }

                return;
            }

            hfAssessmentId.SetValue( assessment.Id );

            // If assessment is completed show the results
            if ( assessment.Status == AssessmentRequestStatus.Complete )
            {
                EQInventoryService.AssessmentResults savedScores = EQInventoryService.LoadSavedAssessmentResults( _targetPerson );
                ShowResult( savedScores, assessment );
                return;
            }

            if ( assessment.Status == AssessmentRequestStatus.Pending )
            {
                if ( _targetPerson.Id != CurrentPerson.Id )
                {
                    // If assessment is pending and the current person is not the one assigned the show previouslyCompletedAssessment results
                    if ( previouslyCompletedAssessment != null )
                    {
                        EQInventoryService.AssessmentResults savedScores = EQInventoryService.LoadSavedAssessmentResults( _targetPerson );
                        ShowResult( savedScores, previouslyCompletedAssessment, true );
                        return;
                    }

                    // If assessment is pending and the current person is not the one assigned and previouslyCompletedAssessment is null show a message that the test has not been completed.
                    HidePanelsAndShowError( string.Format("{0} has not yet taken the {1} Assessment.", _targetPerson.FullName, assessmentType.Title ) );
                }
                else
                {
                    // If assessment is pending and the current person is the one assigned then show the questions
                    ShowInstructions();
                }

                return;
            }

            // This should never happen, if the block gets to this point then something is not right
            HidePanelsAndShowError( "Unable to load assessment" );
        }

        /// <summary>
        /// Hides the Instructions and Questions panels and shows the specified error.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        private void HidePanelsAndShowError( string errorMessage )
        {
            pnlInstructions.Visible = false;
            pnlQuestion.Visible = false;
            pnlResult.Visible = false;
            ShowNotification( errorMessage, NotificationBoxType.Danger );
        }

        /// <summary>
        /// Shows the notification.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="notificationBoxType">Type of the notification box.</param>
        private void ShowNotification( string errorMessage, NotificationBoxType notificationBoxType )
        {
            nbError.Visible = true;
            nbError.Text = errorMessage;
            nbError.NotificationBoxType = notificationBoxType;
        }

        /// <summary>
        /// Sets the page title and icon.
        /// </summary>
        private void SetPanelTitleAndIcon()
        {
            string panelTitle = this.GetAttributeValue( AttributeKey.SetPageTitle );
            if ( !string.IsNullOrEmpty( panelTitle ) )
            {
                lTitle.Text = panelTitle;
            }

            string panelIcon = this.GetAttributeValue( AttributeKey.SetPageIcon );
            if ( !string.IsNullOrEmpty( panelIcon ) )
            {
                iIcon.Attributes["class"] = panelIcon;
            }
        }

        /// <summary>
        /// Shows the instructions.
        /// </summary>
        private void ShowInstructions()
        {
            pnlInstructions.Visible = true;
            pnlQuestion.Visible = false;
            pnlResult.Visible = false;
            
            // Resolve the text field merge fields
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, _targetPerson );
            if ( _targetPerson != null )
            {
                mergeFields.Add( "Person", _targetPerson );
            }

            lInstructions.Text = GetAttributeValue( AttributeKey.Instructions ).ResolveMergeFields( mergeFields );
        }

        /// <summary>
        /// Shows the result.
        /// </summary>
        private void ShowResult( EQInventoryService.AssessmentResults result, Assessment assessment, bool isPrevious = false )
        {
            pnlInstructions.Visible = false;
            pnlQuestion.Visible = false;
            pnlResult.Visible = true;
            btnRetakeTest.Visible = false;

            if ( isPrevious )
            {
                ShowNotification( "A more recent assessment request has been made but has not been taken. Displaying the most recently completed test.", NotificationBoxType.Info );
            }

            bool requiresRequest = assessment.AssessmentType.RequiresRequest;
            var minDays = assessment.AssessmentType.MinimumDaysToRetake;

            if ( !_isQuerystringPersonKey && !requiresRequest && assessment.CompletedDateTime.HasValue && assessment.CompletedDateTime.Value.AddDays( minDays ) <= RockDateTime.Now )
            {
                btnRetakeTest.Visible = true;
            }

            // Resolve the text field merge fields
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, _targetPerson );
            if ( _targetPerson != null )
            {
                _targetPerson.LoadAttributes();
                mergeFields.Add( "Person", _targetPerson );

                // The five Mode scores
                mergeFields.Add( "SelfAwareness", result.SelfAwareConstruct );
                mergeFields.Add( "SelfRegulating", result.SelfRegulatingConstruct );
                mergeFields.Add( "OthersAwareness", result.OtherAwarenessContruct );
                mergeFields.Add( "OthersRegulating", result.OthersRegulatingConstruct );
                mergeFields.Add( "EQinProblemSolving", result.EQ_ProblemSolvingScale );
                mergeFields.Add( "EQUnderStress", result.EQ_UnderStressScale );
            }

            lResult.Text = GetAttributeValue( AttributeKey.ResultsMessage ).ResolveMergeFields( mergeFields );
        }

        /// <summary>
        /// Shows the questions.
        /// </summary>
        private void ShowQuestions()
        {
            pnlInstructions.Visible = false;
            pnlQuestion.Visible = true;
            pnlResult.Visible = false;
            _assessmentResponses = EQInventoryService.GetQuestions()
                                    .Select( a => new AssessmentResponse()
                                    {
                                        Code = a.Key,
                                        Question = a.Value
                                    } ).ToList();

            // If _maxQuestions has not been set yet...
            if ( QuestionCount == 0 && _assessmentResponses != null )
            {
                // Set the max number of questions to be no greater than the actual number of questions.
                int numQuestions = this.GetAttributeValue( AttributeKey.NumberOfQuestions ).AsInteger();
                QuestionCount = ( numQuestions > _assessmentResponses.Count ) ? _assessmentResponses.Count : numQuestions;
            }

            BindRepeater( 0 );
        }

        /// <summary>
        /// Binds the question data to the rQuestions repeater control.
        /// </summary>
        private void BindRepeater( int pageNumber )
        {
            hfPageNo.SetValue( pageNumber );

            var answeredQuestionCount = _assessmentResponses.Where( a => a.Response.HasValue ).Count();
            PercentComplete = Math.Round( ( Convert.ToDecimal( answeredQuestionCount ) / Convert.ToDecimal( _assessmentResponses.Count ) ) * 100.0m, 2 );

            var skipCount = pageNumber * QuestionCount;

            var questions = _assessmentResponses
                .Skip( skipCount )
                .Take( QuestionCount + 1 )
                .ToList();

            rQuestions.DataSource = questions.Take( QuestionCount );
            rQuestions.DataBind();

            // set next button
            if ( questions.Count() > QuestionCount )
            {
                btnNext.Text = "Next";
                btnNext.CommandArgument = "Next";
            }
            else
            {
                btnNext.Text = "Finish";
                btnNext.CommandArgument = "Finish";
            }

            // build prev button
            if ( pageNumber == 0 )
            {
                btnPrevious.Visible = btnPrevious.Enabled = false;
            }
            else
            {
                btnPrevious.Visible = btnPrevious.Enabled = true;
            }
        }

        /// <summary>
        /// Gets the response to the rQuestions repeater control.
        /// </summary>
        private void GetResponse()
        {
            foreach ( var item in rQuestions.Items.OfType<RepeaterItem>() )
            {
                HiddenField hfQuestionCode = item.FindControl( "hfQuestionCode" ) as HiddenField;
                RockRadioButtonList rblQuestion = item.FindControl( "rblQuestion" ) as RockRadioButtonList;
                var assessment = _assessmentResponses.SingleOrDefault( a => a.Code == hfQuestionCode.Value );
                if ( assessment != null )
                {
                    assessment.Response = rblQuestion.SelectedValueAsInt( false );
                }
            }
        }

        #endregion

        #region nested classes

        [Serializable]
        public class AssessmentResponse
        {
            /// <summary>
            /// Gets or sets the code.
            /// </summary>
            /// <value>
            /// The code.
            /// </value>
            public string Code { get; set; }

            /// <summary>
            /// Gets or sets the question.
            /// </summary>
            /// <value>
            /// The question.
            /// </value>
            public string Question { get; set; }

            /// <summary>
            /// Gets or sets the response.
            /// </summary>
            /// <value>
            /// The response.
            /// </value>
            public int? Response { get; set; }
        }

        #endregion
    }
}