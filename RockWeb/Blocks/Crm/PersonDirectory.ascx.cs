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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using System.Data.Entity;
using System.Text;

namespace RockWeb.Blocks.Crm
{
    /// <summary>
    /// A directory of people in database.
    /// </summary>
    [DisplayName( "Person Directory" )]
    [Category( "CRM" )]
    [Description( "A directory of people in database." )]

    #region Block Attributes

    [DataViewField(
        "Data View",
        Key = AttributeKey.DataView,
        Description = "The data view to use as the source for the directory. Only those people returned by the data view filter will be displayed on this directory.",
        IsRequired = true,
        DefaultValue = "cb4bb264-a1f4-4edb-908f-2ccf3a534bc7",
        EntityTypeName = "Rock.Model.Person",
        Order = 0 )]

    [GroupField(
        "Opt-out Group",
        Key = AttributeKey.OptOut,
        Description = "A group that contains people that should be excluded from this list.",
        IsRequired = false,
        Order = 1 )]

    [CustomRadioListField(
        "Show By",
        Key = AttributeKey.ShowBy,
        Description = "People can be displayed individually, or grouped by family.",
        ListSource = "Individual,Family",
        IsRequired = true,
        DefaultValue = "Individual",
        Order = 2 )]

    [BooleanField(
        "Show All People",
        Key = AttributeKey.ShowAllPeople,
        Description = "Display all people by default? If false, a search is required first, and only those matching search criteria will be displayed.",
        DefaultBooleanValue = false,
        Order = 3 )]

    [LinkedPage(
        "Person Profile Page",
        Key = AttributeKey.PersonProfilePage,
        Description = "Page to navigate to when clicking a person's name (leave blank if link should not be enabled).",
        IsRequired = false,
        Order = 4 )]

    [IntegerField(
        "First Name Characters Required",
        Key = AttributeKey.FirstNameCharactersRequired,
        Description = "The number of characters that need to be entered before allowing a search.",
        IsRequired = false,
        DefaultIntegerValue = 1,
        Order = 5 )]

    [IntegerField(
        "Last Name Characters Required",
        Key = AttributeKey.LastNameCharactersRequired,
        Description = "The number of characters that need to be entered before allowing a search.",
        IsRequired = false,
        DefaultIntegerValue = 3,
        Order = 6 )]

    [BooleanField(
        "Show Email",
        Key = AttributeKey.ShowEmail,
        Description = "Should email address be included in the directory?",
        DefaultBooleanValue = true,
        Order = 7 )]

    [BooleanField(
        "Show Address",
        Key = AttributeKey.ShowAddress,
        Description = "Should email address be included in the directory?",
        DefaultBooleanValue = true,
        Order = 8 )]

    [DefinedValueField(
        "Show Phones",
        Key = AttributeKey.ShowPhones,
        Description = "The phone numbers to be included in the directory.",
        DefinedTypeGuid = Rock.SystemGuid.DefinedType.PERSON_PHONE_TYPE,
        IsRequired = false,
        AllowMultiple = true,
        Order = 9 )]

    [BooleanField(
        "Show Birthday",
        Key = AttributeKey.ShowBirthday,
        Description = "Should email address be included in the directory?",
        DefaultBooleanValue = true,
        Order = 10 )]

    [BooleanField(
        "Show Gender",
        Key = AttributeKey.ShowGender,
        Description = "Should email address be included in the directory?",
        DefaultBooleanValue = true,
        Order = 11 )]

    [BooleanField(
        "Show Grade",
        Key = AttributeKey.ShowGrade,
        Description = "Should grade be included in the directory?",
        DefaultBooleanValue = false,
        Order = 12 )]

    [BooleanField(
        "Show Envelope Number",
        Key = AttributeKey.ShowEnvelopeNumber,
        Description = "Should envelope # be included in the directory?",
        DefaultBooleanValue = false,
        Order = 13 )]

    [IntegerField(
        "Max Results",
        Key = AttributeKey.MaxResults,
        Description = "The maximum number of results to show on the page.",
        IsRequired = true,
        DefaultIntegerValue = 1500,
        Order = 14 )]

    #endregion Block Attributes

    [Rock.SystemGuid.BlockTypeGuid( "FAA234E0-9B34-4539-9987-F15E3318B4FF" )]
    public partial class PersonDirectory : Rock.Web.UI.RockBlock
    {
        #region Attribute Keys
        private static class AttributeKey
        {
            public const string DataView = "DataView";
            public const string OptOut = "OptOut";
            public const string ShowBy = "ShowBy";
            public const string ShowAllPeople = "ShowAllPeople";
            public const string FirstNameCharactersRequired = "FirstNameCharactersRequired";
            public const string LastNameCharactersRequired = "LastNameCharactersRequired";
            public const string ShowEmail = "ShowEmail";
            public const string ShowAddress = "ShowAddress";
            public const string ShowBirthday = "ShowBirthday";
            public const string ShowGender = "ShowGender";
            public const string ShowGrade = "ShowGrade";
            public const string ShowEnvelopeNumber = "ShowEnvelopeNumber";
            public const string PersonProfilePage = "PersonProfilePage";
            public const string ShowPhones = "ShowPhones";
            public const string MaxResults = "MaxResults";
        }
        #endregion Attribute Keys

        #region Fields

        private Guid? _dataViewGuid = null;
        private Guid? _optOutGroupGuid = null;
        private bool _showFamily = false;
        private bool _showAllPeople = false;
        private string _personProfileUrl = string.Empty;
        private int? _firsNameCharsRequired = 1;
        private int? _lastNameCharsRequired = 3;
        private bool _showEmail = true;
        private bool _showAddress = true;
        private bool _showBirthday = true;
        private bool _showGender = true;
        private bool _showGrade = false;
        private bool _showEnvelopeNumber = false;
        private Dictionary<int, string> _phoneNumberCaptions = new Dictionary<int, string>();

        private List<PhoneNumber> _phoneNumbers = null;
        private Dictionary<int, List<Location>> _addresses = null;
        private Dictionary<int, List<PersonDirectoryItem>> _familyMembers = null;
        private Dictionary<int, string> _envelopeNumbers = null;

        protected string _peopleCol1Class = "col-md-5";
        protected string _peopleCol2Class = "col-md-3";
        protected string _peopleCol3Class = "col-md-2";
        protected string _peopleCol4Class = "col-md-2";

        protected string _familyCol1Class = "col-md-6";
        protected string _familyCol2Class = "col-md-3";
        protected string _familyCol3Class = "col-md-3";

        #endregion

        #region Properties
        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage.AddScriptLink( "~/Scripts/jquery.lazyload.min.js" );

            rptPeople.ItemDataBound += rptPeople_ItemDataBound;
            rptFamilies.ItemDataBound += rptFamilies_ItemDataBound;

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            _dataViewGuid = GetAttributeValue( AttributeKey.DataView ).AsGuidOrNull();
            _optOutGroupGuid = GetAttributeValue( AttributeKey.OptOut ).AsGuidOrNull();
            _showFamily = GetAttributeValue( AttributeKey.ShowBy ) == "Family";
            _showAllPeople = GetAttributeValue( AttributeKey.ShowAllPeople ).AsBoolean();
            _firsNameCharsRequired = GetAttributeValue( AttributeKey.FirstNameCharactersRequired ).AsIntegerOrNull();
            _lastNameCharsRequired = GetAttributeValue( AttributeKey.LastNameCharactersRequired ).AsIntegerOrNull();
            _showEmail = GetAttributeValue( AttributeKey.ShowEmail ).AsBoolean();
            _showAddress = GetAttributeValue( AttributeKey.ShowAddress ).AsBoolean();
            _showBirthday = GetAttributeValue( AttributeKey.ShowBirthday ).AsBoolean();
            _showGender = GetAttributeValue( AttributeKey.ShowGender ).AsBoolean();
            _showGrade = GetAttributeValue( AttributeKey.ShowGrade ).AsBoolean();
            _showEnvelopeNumber = GetAttributeValue( AttributeKey.ShowEnvelopeNumber ).AsBoolean();
            _personProfileUrl = LinkedPageUrl( AttributeKey.PersonProfilePage, new Dictionary<string, string> { { "PersonId", "999" } } ).Replace( "999", "{0}" );

            foreach ( var guid in GetAttributeValue( AttributeKey.ShowPhones ).SplitDelimitedValues().AsGuidList() )
            {
                var phoneValue = DefinedValueCache.Get( guid );
                if ( phoneValue != null )
                {
                    _phoneNumberCaptions.Add( phoneValue.Id, phoneValue.Value );
                }
            }

            phLetters.Visible = _showAllPeople;

            lbOptInOut.Visible = CurrentPerson != null && _optOutGroupGuid.HasValue;
            pnlOptOut.Visible = lbOptInOut.Visible;

            BuildLetters();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            nbValidation.Visible = false;

            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                BindData();
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            BindData();
        }

        protected void lbSearch_Click( object sender, EventArgs e )
        {
            var msgs = new List<string>();

            if ( _firsNameCharsRequired.HasValue && tbFirstName.Text.Trim().Length < _firsNameCharsRequired.Value )
            {
                msgs.Add( string.Format( "Please enter at least {0:N0} {1} in First Name before searching.",
                    _firsNameCharsRequired.Value, "character".PluralizeIf( _firsNameCharsRequired.Value > 1 ) ) );
            }

            if ( _lastNameCharsRequired.HasValue && tbLastName.Text.Trim().Length < _lastNameCharsRequired.Value )
            {
                msgs.Add( string.Format( "Please enter at least {0:N0} {1} in Last Name before searching.",
                    _lastNameCharsRequired.Value, "character".PluralizeIf( _lastNameCharsRequired.Value > 1 ) ) );
            }

            if ( msgs.Any() )
            {
                ShowMessages( msgs );
            }
            else
            {
                BindData();
            }
        }

        protected void lbClear_Click( object sender, EventArgs e )
        {
            tbFirstName.Text = string.Empty;
            tbLastName.Text = string.Empty;

            BindData();
        }

        private void lbLetter_Click( object sender, EventArgs e )
        {
            var lbLetter = sender as LinkButton;
            if ( lbLetter != null )
            {
                tbFirstName.Text = string.Empty;
                tbLastName.Text = lbLetter.ID.Substring( 8 );
            }

            BindData();
        }

        private void rptPeople_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var personItem = e.Item.DataItem as PersonDirectoryItem;
            if ( personItem != null )
            {
                var lPerson = e.Item.FindControl( "lPerson" ) as Literal;
                var lAddress = e.Item.FindControl( "lAddress" ) as Literal;
                var lPhones = e.Item.FindControl( "lPhones" ) as Literal;
                var lEverythingElse = e.Item.FindControl( "lEverythingElse" ) as Literal;

                if ( lPerson != null )
                {
                    var sb = new StringBuilder();

                    string personName = personItem.NickName + " " + personItem.LastName;
                    personName = string.IsNullOrWhiteSpace( _personProfileUrl ) ? personName : string.Format( "<a href='{0}'>{1}</a>", string.Format( _personProfileUrl, personItem.Id ), personName );

                    sb.Append( string.Format( "<div class=\"photo-round photo-round-sm pull-left\" data-original=\"{0}&w=100\" style=\"background-image: url('{1}');\"></div>", personItem.PhotoUrl, ResolveUrl( "~/Assets/Images/person-no-photo-unknown.svg" ) ) );
                    sb.Append( "<div class=\"pull-left margin-l-sm\">" );
                    sb.AppendFormat( "<strong>{0}</strong>", personName );

                    if ( _showEmail && !string.IsNullOrWhiteSpace( personItem.Email ) )
                    {
                        sb.AppendFormat( "<br/>{0}", personItem.Email );
                    }

                    sb.Append( "</div>" );

                    lPerson.Text = string.Format( "<div class='{0} clearfix'>{1}</div>",
                        _showFamily ? _familyCol1Class : _peopleCol1Class, sb.ToString() );
                }

                if ( _showAddress && _addresses != null && lAddress != null )
                {
                    var formattedAddresses = new List<string>();
                    if ( _addresses.ContainsKey( personItem.Id ) )
                    {
                        _addresses[personItem.Id].ForEach( a => formattedAddresses.Add( a.FormattedHtmlAddress ) );
                    }

                    lAddress.Text = string.Format( "<div class='{0} clearfix'>{1}</div>",
                        _peopleCol2Class, formattedAddresses.AsDelimited( "<br/><br/>" ) );
                }

                if ( _phoneNumberCaptions.Any() && _phoneNumbers != null && lPhones != null )
                {
                    var phones = _phoneNumbers
                        .Where( p => p.PersonId == personItem.Id )
                        .Select( p => string.Format( "{0} <small>{1}</small>", p.NumberFormatted, _phoneNumberCaptions[p.NumberTypeValueId.Value] ) )
                        .ToList();

                    lPhones.Text = string.Format( "<div class='{0} clearfix'>{1}</div>",
                        _showFamily ? _familyCol2Class : _peopleCol3Class, phones.AsDelimited( "<br/>" ) );
                }

                if ( ( _showBirthday || _showGender || _showGrade || _showEnvelopeNumber ) && lEverythingElse != null )
                {
                    var sb = new StringBuilder();
                    if ( _showBirthday )
                    {
                        if ( personItem.BirthMonth.HasValue && personItem.BirthDay.HasValue )
                        {
                            DateTime bd = new DateTime( 2000, personItem.BirthMonth.Value, personItem.BirthDay.Value );
                            sb.AppendFormat( "Birthday: {0:MMM d}", bd );
                        }
                    }

                    if ( _showGender && personItem.Gender != Gender.Unknown )
                    {
                        if ( sb.Length > 0 )
                        {
                            sb.Append( "<br/>" );
                        }

                        sb.AppendFormat( "Gender: {0}", personItem.Gender == Gender.Male ? "M" : "F" );
                    }

                    if ( _showGrade )
                    {
                        if ( !string.IsNullOrWhiteSpace( personItem.Grade ) )
                        {
                            if ( sb.Length > 0 )
                            {
                                sb.Append( "<br/>" );
                            }

                            sb.AppendFormat( "Grade: {0}", personItem.Grade );
                        }
                    }

                    if ( _showEnvelopeNumber && _envelopeNumbers != null )
                    {
                        var envelopeNumber = _envelopeNumbers.ContainsKey( personItem.Id ) ? _envelopeNumbers[personItem.Id] : null;
                        if ( !string.IsNullOrWhiteSpace( envelopeNumber ) )
                        {
                            if ( sb.Length > 0 )
                            {
                                sb.Append( "<br/>" );
                            }

                            sb.AppendFormat( "Envelope #: {0}", envelopeNumber );
                        }
                    }

                    lEverythingElse.Text = string.Format( "<div class='{0} clearfix'>{1}</div>",
                        _showFamily ? _familyCol3Class : _peopleCol4Class, sb.ToString() );
                }
            }
        }

        private void rptFamilies_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var familyItem = e.Item.DataItem as FamilyDirectoryItem;
            if ( familyItem != null && _familyMembers != null && _familyMembers.ContainsKey( familyItem.Id ) )
            {
                var lFamily = e.Item.FindControl( "lFamily" ) as Literal;
                var rptFamilyPeople = e.Item.FindControl( "rptFamilyPeople" ) as Repeater;

                if ( lFamily != null )
                {
                    var sb = new StringBuilder();
                    sb.Append( string.Format( "<strong>{0}</strong>", familyItem.Name ) );

                    if ( _showAddress && _addresses != null )
                    {
                        sb.Append( "<br/>" );

                        var formattedAddresses = new List<string>();
                        if ( _addresses.ContainsKey( familyItem.Id ) )
                        {
                            _addresses[familyItem.Id].ForEach( a => formattedAddresses.Add( a.FormattedHtmlAddress ) );
                        }

                        sb.Append( formattedAddresses.AsDelimited( "<br/><br/>" ) );
                    }

                    lFamily.Text = sb.ToString();
                }

                if ( rptFamilyPeople != null )
                {
                    rptFamilyPeople.ItemDataBound += rptPeople_ItemDataBound;
                    rptFamilyPeople.DataSource = _familyMembers[familyItem.Id];
                    rptFamilyPeople.DataBind();
                }
            }
        }

        protected void btnOptOutIn_Click( object sender, EventArgs e )
        {
            if ( CurrentPerson != null && _optOutGroupGuid.HasValue )
            {
                using ( var rockContext = new RockContext() )
                {
                    bool optIn = lbOptInOut.Text == "Opt in to the Directory";
                    if ( _showFamily )
                    {
                        var familyGroupType = GroupTypeCache.Get( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid() );

                        foreach ( var familyMember in new GroupService( rockContext )
                            .Queryable().AsNoTracking()
                            .Where( g =>
                                g.GroupTypeId == familyGroupType.Id &&
                                g.Members.Any( m => m.PersonId == CurrentPerson.Id ) )
                            .SelectMany( g => g.Members )
                            .Select( m => m.Person ) )
                        {
                            OptInOutPerson( rockContext, CurrentPerson, optIn );
                        }
                    }
                    else
                    {
                        OptInOutPerson( rockContext, CurrentPerson, optIn );
                    }

                    rockContext.SaveChanges();
                }

                BindData();
            }
        }

        #endregion

        #region Methods

        private void BuildLetters()
        {
            for ( char c = 'A'; c <= 'Z'; c++ )
            {
                LinkButton lbLetter = new LinkButton();
                lbLetter.ID = string.Format( "lbLetter{0}", c );
                lbLetter.Text = c.ToString();
                lbLetter.Click += lbLetter_Click;

                var li = new System.Web.UI.HtmlControls.HtmlGenericControl( "li" );
                li.Controls.Add( lbLetter );

                phLetters.Controls.Add( li );
            }
        }

        private void BindData()
        {
            using ( var rockContext = new RockContext() )
            {
                var dataView = new DataViewService( rockContext ).Get( _dataViewGuid ?? Guid.Empty );
                if ( dataView == null )
                {
                    rptPeople.Visible = false;
                    rptFamilies.Visible = false;
                    ShowMessages( new List<string> { "This block requires a valid Data View setting." } );
                    return;
                }

                var personService = new PersonService( rockContext );

                // Filter people by dataview
                var paramExpression = personService.ParameterExpression;
                var whereExpression = dataView.GetExpression( personService, paramExpression );
                var personQry = personService
                    .Queryable( false, false ).AsNoTracking()
                    .Where( paramExpression, whereExpression, null );

                var dvPersonIdQry = personQry.Select( p => p.Id );

                bool useFilteredQuery = false;

                // Filter by first name
                string firstName = tbFirstName.Text.Trim();
                if ( !string.IsNullOrWhiteSpace( firstName ) )
                {
                    personQry = personQry.Where( p =>
                        p.FirstName.StartsWith( firstName ) ||
                        p.NickName.StartsWith( firstName ) );
                    useFilteredQuery = true;
                }

                // Filter by last name
                string lastName = tbLastName.Text.Trim();
                if ( !string.IsNullOrWhiteSpace( lastName ) )
                {
                    personQry = personQry.Where( p =>
                        p.LastName.StartsWith( lastName ) );
                    useFilteredQuery = true;
                }


                if ( useFilteredQuery || _showAllPeople )
                {
                    SetColumnWidths();

                    if ( _optOutGroupGuid.HasValue )
                    {
                        var optOutPersonIdQry = new GroupMemberService( rockContext )
                            .Queryable().AsNoTracking()
                            .Where( g => g.Group.Guid.Equals( _optOutGroupGuid.Value ) )
                            .Select( g => g.PersonId );

                        personQry = personQry.Where( p =>
                            !optOutPersonIdQry.Contains( p.Id ) );
                    }

                    if ( _showFamily )
                    {
                        BindFamilies( rockContext, personQry, dvPersonIdQry );
                    }
                    else
                    {
                        BindPeople( rockContext, personQry );
                    }

                }
                else
                {
                    rptPeople.Visible = false;
                    rptFamilies.Visible = false;
                }

                if ( CurrentPerson != null && _optOutGroupGuid.HasValue )
                {
                    bool optedOut = new GroupMemberService( rockContext )
                            .Queryable().AsNoTracking()
                            .Any( m =>
                                m.PersonId == CurrentPerson.Id &&
                                m.Group.Guid.Equals( _optOutGroupGuid.Value ) );
                    lbOptInOut.Text = optedOut ? "Opt in to the Directory" : "Opt Out of the Directory";
                }
            }
        }

        private void BindPeople( RockContext rockContext, IQueryable<Person> personQry )
        {
            var people = personQry
                .OrderBy( p => p.LastName )
                .ThenBy( p => p.NickName )
                .Take( GetAttributeValue( AttributeKey.MaxResults ).AsInteger() )
                .Select( p => new PersonDirectoryItem
                {
                    Id = p.Id,
                    RecordTypeValueId = p.RecordTypeValueId,
                    AgeClassification = p.AgeClassification,
                    NickName = p.NickName,
                    LastName = p.LastName,
                    Email = p.Email,
                    BirthMonth = p.BirthMonth,
                    BirthDay = p.BirthDay,
                    BirthDate = p.BirthDate,
                    DeceasedDate = p.DeceasedDate,
                    Gender = p.Gender,
                    PhotoId = p.PhotoId,
                    GraduationYear = p.GraduationYear
                } )
                .ToList();

            if ( _showAddress || _phoneNumberCaptions.Any() )
            {
                var personIds = people.Select( p => p.Id ).ToList();

                if ( _showAddress )
                {
                    var familyGroupType = GroupTypeCache.Get( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid() );
                    var homeLoc = DefinedValueCache.Get( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME.AsGuid() );
                    if ( familyGroupType != null && homeLoc != null )
                    {
                        _addresses = new GroupMemberService( rockContext )
                            .Queryable().AsNoTracking()
                            .Where( m =>
                                personIds.Contains( m.PersonId ) &&
                                m.Group.GroupTypeId == familyGroupType.Id )
                            .Select( m => new
                            {
                                m.PersonId,
                                HomeLocations = m.Group.GroupLocations
                                    .Where( gl =>
                                        gl.GroupLocationTypeValueId.HasValue &&
                                        gl.GroupLocationTypeValueId == homeLoc.Id )
                                    .Select( gl => gl.Location )
                                    .ToList()
                            } )
                            .GroupBy( m => m.PersonId )
                            .Select( g => new
                            {
                                PersonId = g.Key,
                                HomeLocations = g.SelectMany( m => m.HomeLocations ).ToList()
                            } )
                            .ToDictionary( k => k.PersonId, v => v.HomeLocations );
                    }
                }

                LoadPhoneNumbers( rockContext, personIds );
            }

            if ( _showEnvelopeNumber && GlobalAttributesCache.Get().EnableGivingEnvelopeNumber )
            {
                LoadEnvelopeNumbers( rockContext, people.Select( p => p.Id ).ToList() );
            }

            rptPeople.DataSource = people;
            rptPeople.DataBind();
            rptPeople.Visible = true;

            rptFamilies.Visible = false;
        }

        /// <summary>
        /// Loads the envelope numbers.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="personIds">The person ids.</param>
        private void LoadEnvelopeNumbers( RockContext rockContext, List<int> personIds )
        {
            var personGivingEnvelopeAttribute = AttributeCache.Get( Rock.SystemGuid.Attribute.PERSON_GIVING_ENVELOPE_NUMBER.AsGuid() );
            _envelopeNumbers = new AttributeValueService( rockContext ).Queryable()
                                    .Where( a => a.AttributeId == personGivingEnvelopeAttribute.Id )
                                    .Where( a => personIds.Contains( a.EntityId.Value ) )
                                    .Select( a => new
                                    {
                                        PersonId = a.EntityId.Value,
                                        Value = a.Value
                                    } ).ToList().ToDictionary( k => k.PersonId, v => v.Value );
        }

        private void BindFamilies( RockContext rockContext, IQueryable<Person> personQry, IQueryable<int> dataviewPersonIdQry )
        {
            var familyGroupType = GroupTypeCache.Get( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid() );

            var personIds = personQry.Select( p => p.Id );

            var groupMemberQry = new GroupMemberService( rockContext )
                .Queryable().AsNoTracking()
                .Where( m =>
                    m.Group.GroupTypeId == familyGroupType.Id &&
                    personIds.Contains( m.PersonId ) );

            var familyIdQry = groupMemberQry.Select( m => m.GroupId ).Distinct();

            _familyMembers = new GroupService( rockContext )
                .Queryable().AsNoTracking()
                .Where( g => familyIdQry.Contains( g.Id ) )
                .Select( g => new
                {
                    GroupId = g.Id,
                    People = g.Members
                        .Where( m => dataviewPersonIdQry.Contains( m.PersonId ) )
                        .OrderBy( m => m.GroupRole.Order )
                        .ThenBy( m => m.Person.BirthDate )
                        .Select( m => m.Person )
                        .Select( p => new PersonDirectoryItem
                        {
                            Id = p.Id,
                            RecordTypeValueId = p.RecordTypeValueId,
                            NickName = p.NickName,
                            LastName = p.LastName,
                            Email = p.Email,
                            BirthMonth = p.BirthMonth,
                            BirthDay = p.BirthDay,
                            BirthDate = p.BirthDate,
                            DeceasedDate = p.DeceasedDate,
                            Gender = p.Gender,
                            PhotoId = p.PhotoId,
                            GraduationYear = p.GraduationYear
                        } )
                        .ToList()
                } )
                .ToDictionary( k => k.GroupId, v => v.People );

            if ( _showAddress )
            {
                var homeLoc = DefinedValueCache.Get( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME.AsGuid() );
                if ( familyGroupType != null && homeLoc != null )
                {
                    _addresses = new GroupService( rockContext )
                        .Queryable().AsNoTracking()
                        .Where( g => familyIdQry.Contains( g.Id ) )
                        .Select( g => new
                        {
                            g.Id,
                            HomeLocations = g.GroupLocations
                                .Where( gl =>
                                    gl.GroupLocationTypeValueId.HasValue &&
                                    gl.GroupLocationTypeValueId == homeLoc.Id )
                                .Select( gl => gl.Location )
                                .ToList()
                        } )
                        .ToDictionary( k => k.Id, v => v.HomeLocations );
                }
            }

            var familyPersonIds = _familyMembers
                    .SelectMany( m => m.Value )
                    .Select( p => p.Id )
                    .Distinct()
                    .ToList();

            if ( _phoneNumberCaptions.Any() )
            {
                LoadPhoneNumbers( rockContext, familyPersonIds );
            }

            if ( _showEnvelopeNumber && GlobalAttributesCache.Get().EnableGivingEnvelopeNumber )
            {
                LoadEnvelopeNumbers( rockContext, familyPersonIds );
            }

            var families = groupMemberQry.Select( m => new FamilyDirectoryItem
            {
                Id = m.GroupId,
                Name = m.Group.Name
            } )
            .Distinct()
            .OrderBy( f => f.Name )
            .Take( GetAttributeValue( AttributeKey.MaxResults ).AsInteger() )
            .ToList();

            rptFamilies.DataSource = families;
            rptFamilies.DataBind();
            rptFamilies.Visible = true;

            rptPeople.Visible = false;
        }

        private void LoadPhoneNumbers( RockContext rockContext, List<int> personIds )
        {
            if ( _phoneNumberCaptions.Any() )
            {
                _phoneNumbers = new PhoneNumberService( rockContext )
                    .Queryable().AsNoTracking()
                    .Where( p =>
                        personIds.Contains( p.PersonId ) &&
                        !p.IsUnlisted &&
                        p.NumberTypeValueId.HasValue &&
                        _phoneNumberCaptions.Keys.Contains( p.NumberTypeValueId.Value ) )
                    .OrderBy( p => p.PersonId )
                    .ThenBy( p => p.NumberTypeValue.Order )
                    .ToList();
            }
        }

        private void OptInOutPerson( RockContext rockContext, Person person, bool optIn )
        {
            var groupMemberService = new GroupMemberService( rockContext );
            var groupMembers = groupMemberService
                .Queryable()
                .Where( m =>
                    m.PersonId == person.Id &&
                    m.Group.Guid.Equals( _optOutGroupGuid.Value ) )
                .ToList();

            if ( !optIn && !groupMembers.Any() )
            {
                var optOutGroup = new GroupService( rockContext ).Get( _optOutGroupGuid.Value );
                if ( optOutGroup != null )
                {
                    var groupMember = new GroupMember();
                    groupMember.GroupId = optOutGroup.Id;
                    groupMember.PersonId = person.Id;
                    groupMember.GroupRoleId = optOutGroup.GroupType.DefaultGroupRoleId ?? 0;
                    optOutGroup.Members.Add( groupMember );
                }
            }

            if ( optIn && groupMembers.Any() )
            {
                foreach ( var groupMember in groupMembers )
                {
                    groupMemberService.Delete( groupMember );
                }
            }
        }

        private void ShowMessages( List<string> messages )
        {
            if ( messages.Any() )
            {
                nbValidation.Text = string.Format( "Please correct the following: <ul><li>{0}</li></ul>",
                    messages.AsDelimited( "</li><li>" ) );
                nbValidation.Visible = true;
            }
        }

        private void SetColumnWidths()
        {
            int peopleCols = 1;
            int familyCols = 1;

            if ( _showAddress )
            {
                peopleCols++;
            }
            if ( _phoneNumberCaptions.Any() )
            {
                peopleCols++;
                familyCols++;
            }
            if ( _showBirthday || _showGender )
            {
                peopleCols++;
                familyCols++;
            }

            if ( _showFamily )
            {
                if ( familyCols < 3 )
                {
                    _familyCol1Class = "col-md-6";
                    _familyCol2Class = "col-md-6";
                    _familyCol3Class = "col-md-6";

                    if ( peopleCols < 2 )
                    {
                        _familyCol1Class = "col-md-12";
                    }
                }
            }
            else
            {
                if ( peopleCols < 4 )
                {
                    _peopleCol1Class = "col-md-6";
                    _peopleCol2Class = "col-md-3";
                    _peopleCol3Class = "col-md-3";
                    _peopleCol4Class = "col-md-3";

                    if ( peopleCols < 3 )
                    {
                        _peopleCol1Class = "col-md-6";
                        _peopleCol2Class = "col-md-6";
                        _peopleCol3Class = "col-md-6";
                        _peopleCol4Class = "col-md-6";

                        if ( peopleCols < 2 )
                        {
                            _peopleCol1Class = "col-md-12";
                        }
                    }
                }
            }
        }

        #endregion

        public class FamilyDirectoryItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class PersonDirectoryItem
        {
            public int Id { get; set; }
            public int? RecordTypeValueId { get; set; }
            public AgeClassification AgeClassification { get; set; }
            public string NickName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int? BirthMonth { get; set; }
            public int? BirthDay { get; set; }
            public DateTime? BirthDate { get; set; }
            public DateTime? DeceasedDate { get; set; }
            public int? Age
            {
                get
                {
                    return Person.GetAge( this.BirthDate, this.DeceasedDate );
                }
            }
            public Gender Gender { get; set; }
            public int? GraduationYear { get; set; }
            public string Grade
            {
                get
                {
                    return Person.GradeFormattedFromGraduationYear( this.GraduationYear );
                }
            }
            public int? PhotoId { get; set; }
            public string PhotoUrl
            {
                get
                {
                    if ( this.RecordTypeValueId.HasValue )
                    {
                        var recordType = DefinedValueCache.Get( RecordTypeValueId.Value );
                        if ( recordType != null )
                        {
                            return Person.GetPersonPhotoUrl( this.Id, this.PhotoId, this.Age, this.Gender, recordType.Guid, this.AgeClassification, 200, 200 );
                        }
                    }
                    return Person.GetPersonPhotoUrl( this.Id, this.PhotoId, this.Age, this.Gender, null, this.AgeClassification, 200, 200 );
                }
            }

        }
    }
}