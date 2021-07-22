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
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Lava;
using Rock.Lava.DotLiquid;
using Rock.Lava.Fluid;
using Rock.Lava.RockLiquid;
using Rock.Tests.Shared;
using Rock.Utility;

namespace Rock.Tests.UnitTests.Lava
{
    public class LavaUnitTestHelper
    {
        private static LavaUnitTestHelper _instance = null;
        public static LavaUnitTestHelper CurrentInstance
        {
            get
            {
                if ( _instance == null )
                {
                    throw new Exception( "Helper not configured. Call the Initialize() method to initialize this helper before use." );
                }

                return _instance;
            }

        }
        public static bool FluidEngineIsEnabled { get; set; }
        public static bool DotLiquidEngineIsEnabled { get; set; }
        public static bool RockLiquidEngineIsEnabled { get; set; }

        private static ILavaEngine _rockliquidEngine = null;
        private static ILavaEngine _dotliquidEngine = null;
        private static ILavaEngine _fluidEngine = null;

        public static void Initialize( bool testRockLiquidEngine, bool testDotLiquidEngine, bool testFluidEngine )
        {
            // Verify the test environment: RockLiquidEngine and DotLiquidEngine are mutually exclusive test environments.
            if ( testRockLiquidEngine && testDotLiquidEngine )
            {
                throw new Exception( "RockLiquidEngine/DotLiquidEngine cannot be tested simultaneously because they require different global configurations of the DotLiquid library." );
            }

            RockLiquidEngineIsEnabled = testRockLiquidEngine;
            DotLiquidEngineIsEnabled = testDotLiquidEngine;
            FluidEngineIsEnabled = testFluidEngine;

            RegisterLavaEngines();

            if ( RockLiquidEngineIsEnabled )
            {
                // Initialize the Rock variant of the DotLiquid Engine
                var engineOptions = new LavaEngineConfigurationOptions();

                _rockliquidEngine = LavaService.NewEngineInstance( typeof (RockLiquidEngine ), engineOptions );

                // Register the common Rock.Lava filters first, then overwrite with the web-based RockFilters as needed.
                RegisterFilters( _rockliquidEngine );
            }

            if ( DotLiquidEngineIsEnabled )
            {
                // Initialize the DotLiquid Engine
                var engineOptions = new LavaEngineConfigurationOptions();

                _dotliquidEngine = LavaService.NewEngineInstance( typeof( DotLiquidEngine ), engineOptions );

                RegisterFilters( _dotliquidEngine );
            }

            if ( FluidEngineIsEnabled )
            {
                // Initialize Fluid Engine
                var engineOptions = new LavaEngineConfigurationOptions();

                _fluidEngine = LavaService.NewEngineInstance( typeof( FluidEngine ), engineOptions );

                RegisterFilters( _fluidEngine );
            }

            _instance = new LavaUnitTestHelper();
        }

        private static void RegisterLavaEngines()
        {
            // Register the RockLiquid Engine (pre-v13).
            LavaService.RegisterEngine( ( engineServiceType, options ) =>
            {
                var engineOptions = new LavaEngineConfigurationOptions();

                var rockLiquidEngine = new RockLiquidEngine();

                rockLiquidEngine.Initialize( engineOptions );

                return rockLiquidEngine;
            } );

            // Register the DotLiquid Engine.
            LavaService.RegisterEngine( ( engineServiceType, options ) =>
            {
                var engineOptions = new LavaEngineConfigurationOptions
                {
                    FileSystem = new WebsiteLavaFileSystem(),
                    CacheService = new WebsiteLavaTemplateCacheService(),
                };

                var dotLiquidEngine = new DotLiquidEngine();

                dotLiquidEngine.Initialize( engineOptions );

                return dotLiquidEngine;
            } );

            // Register the Fluid Engine.
            LavaService.RegisterEngine( ( engineServiceType, options ) =>
            {
                var engineOptions = new LavaEngineConfigurationOptions
                {
                    FileSystem = new WebsiteLavaFileSystem(),
                    CacheService = new WebsiteLavaTemplateCacheService(),
                };

                var fluidEngine = new FluidEngine();

                fluidEngine.Initialize( engineOptions );

                return fluidEngine;
            } );
        }

        private static void RegisterFilters( ILavaEngine engine )
        {
            // Register the common Rock.Lava filters first, then overwrite with the web-specific filters.
            if ( engine.EngineIdentifier == TestGuids.LavaEngines.RockLiquid.AsGuid() )
            {
                engine.RegisterFilters( typeof( global::Rock.Lava.Filters.TemplateFilters ) );
                engine.RegisterFilters( typeof( global::Rock.Lava.RockFilters ) );
            }
            else
            {
                engine.RegisterFilters( typeof( global::Rock.Lava.Filters.TemplateFilters ) );
                engine.RegisterFilters( typeof( global::Rock.Lava.LavaFilters ) );
            }
        }

        private ILavaEngine GetEngineInstance( Type engineType )
        {
            if ( engineType == typeof( FluidEngine ) )
            {
                return _fluidEngine;
            }
            if ( engineType == typeof( DotLiquidEngine ) )
            {
                return _dotliquidEngine;
            }
            else if ( engineType == typeof( RockLiquidEngine ) )
            {
                return _rockliquidEngine;
            }

            throw new Exception( $"Cannot return an instance of engine type \"{ engineType }\"." );
        }

        /// <summary>
        /// Process the specified input template and return the result.
        /// </summary>
        /// <param name="inputTemplate"></param>
        /// <returns></returns>
        public string GetTemplateOutput( Type engineType, string inputTemplate, LavaDataDictionary mergeValues = null )
        {
            var engine = GetEngineInstance( engineType );

            return GetTemplateOutput( engine, inputTemplate, mergeValues );
        }

        /// <summary>
        /// Process the specified input template and return the result.
        /// </summary>
        /// <param name="inputTemplate"></param>
        /// <returns></returns>
        public string GetTemplateOutput( ILavaEngine engine, string inputTemplate, LavaDataDictionary mergeValues = null )
        {
            inputTemplate = inputTemplate ?? string.Empty;

            if ( engine == null )
            {
                throw new Exception( "Engine instance is required." );
            }

            var context = engine.NewRenderContext( mergeValues );

            var result = engine.RenderTemplate( inputTemplate.Trim(), new LavaRenderParameters { Context = context } );

            Assert.That.IsFalse( result.HasErrors, "Lava Template is invalid." );

            return result.Text;
        }

        /// <summary>
        /// For each of the currently enabled Lava Engines, process the specified input template and verify against the expected output.
        /// </summary>
        /// <param name="expectedOutput"></param>
        /// <param name="inputTemplate"></param>
        public void AssertTemplateOutput( string expectedOutput, string inputTemplate, LavaDataDictionary mergeValues = null, bool ignoreWhitespace = false )
        {
            var engines = GetActiveTestEngines();

            foreach ( var engine in engines )
            {
                AssertTemplateOutput( engine, expectedOutput, inputTemplate, mergeValues, ignoreWhitespace );
            }
        }

        /// <summary>
        /// For each of the currently enabled Lava Engines, process the specified input template and verify against the expected output.
        /// </summary>
        /// <param name="expectedOutput"></param>
        /// <param name="inputTemplate"></param>
        public void AssertTemplateOutput( Type engineType, string expectedOutput, string inputTemplate, LavaDataDictionary mergeValues = null, bool ignoreWhitespace = false )
        {
            var engine = GetEngineInstance( engineType );

            AssertTemplateOutput( engine, expectedOutput, inputTemplate, mergeValues, ignoreWhitespace );
        }

        #region Active Engines

        private List<ILavaEngine> _activeEngines = null;

        private List<ILavaEngine> GetActiveTestEngines()
        {
            if ( _activeEngines == null )
            {
                _activeEngines = new List<ILavaEngine>();

                // Test the Fluid engine first, because it is the most likely to fail!
                if ( FluidEngineIsEnabled )
                {
                    _activeEngines.Add( _fluidEngine );
                }

                if ( DotLiquidEngineIsEnabled )
                {
                    _activeEngines.Add( _dotliquidEngine );
                }

                if ( RockLiquidEngineIsEnabled )
                {
                    _activeEngines.Add( _rockliquidEngine );
                }
            }

            return _activeEngines;
        }

        #endregion

        /// <summary>
        /// For each of the currently enabled Lava Engines, register a safe type.
        /// </summary>
        /// <param name="type"></param>
        public void RegisterSafeType( Type type )
        {
            if ( DotLiquidEngineIsEnabled )
            {
                _dotliquidEngine.RegisterSafeType( type );
            }

            if ( FluidEngineIsEnabled )
            {
                _fluidEngine.RegisterSafeType( type );
            }
        }

        /// <summary>
        /// For each of the currently enabled Lava Engines, process the specified action.
        /// </summary>
        /// <param name="expectedOutput"></param>
        /// <param name="inputTemplate"></param>
        public void ExecuteTestAction( Action<ILavaEngine> testMethod )
        {
            var engines = GetActiveTestEngines();

            foreach ( var engine in engines )
            {
                testMethod( engine );
            }
        }

        /// <summary>
        /// For each of the currently enabled Lava Engines, process the specified input template and verify against the expected output.
        /// </summary>
        /// <param name="expectedOutput"></param>
        /// <param name="inputTemplate"></param>
        public void AssertTemplateOutput( ILavaEngine engine, string expectedOutput, string inputTemplate, LavaDataDictionary mergeValues = null, bool ignoreWhitespace = false )
        {
            var outputString = GetTemplateOutput( engine, inputTemplate, mergeValues );

            var debugString = outputString;

            if ( ignoreWhitespace )
            {
                expectedOutput = expectedOutput.RemoveWhiteSpace();
                outputString = outputString.RemoveWhiteSpace();

                debugString += "\n(Comparison ignores WhiteSpace)";
            }

            WriteOutputToDebug( engine, debugString );

            Assert.That.Equal( expectedOutput, outputString );
        }

        /// <summary>
        /// Process the specified input template and verify against the expected output regular expression.
        /// </summary>
        /// <param name="expectedOutputRegex"></param>
        /// <param name="inputTemplate"></param>
        public void AssertTemplateOutputRegex( string expectedOutput, string inputTemplate, LavaDataDictionary mergeValues = null )
        {
            foreach ( var engine in GetActiveTestEngines() )
            {
                AssertTemplateOutputRegex( engine, expectedOutput, inputTemplate, mergeValues );
            }
        }

        /// <summary>
        /// Process the specified input template and verify against the expected output regular expression.
        /// </summary>
        /// <param name="expectedOutputRegex"></param>
        /// <param name="inputTemplate"></param>
        public void AssertTemplateOutputRegex( ILavaEngine engine, string expectedOutputRegex, string inputTemplate, LavaDataDictionary mergeValues = null )
        {
            var outputString = GetTemplateOutput( engine, inputTemplate, mergeValues );

            var regex = new Regex( expectedOutputRegex );

            WriteOutputToDebug( engine, outputString );

            StringAssert.Matches( outputString, regex );
        }

        /// <summary>
        /// Process the specified input template and verify the output against an expected DateTime result.
        /// </summary>
        /// <param name="expectedOutput"></param>
        /// <param name="inputTemplate"></param>
        /// <param name="maximumDelta"></param>
        public void AssertTemplateOutputDate( DateTime? expectedOutput, string inputTemplate, TimeSpan? maximumDelta = null )
        {
            var engines = GetActiveTestEngines();

            foreach ( var engine in engines )
            {
                AssertTemplateOutputDate( engine, expectedOutput, inputTemplate, maximumDelta );
            }
        }

        /// <summary>
        /// Process the specified input template and verify the output against an expected DateTime result.
        /// </summary>
        /// <param name="expectedDateTime"></param>
        /// <param name="inputTemplate"></param>
        /// <param name="maximumDelta"></param>
        public void AssertTemplateOutputDate( ILavaEngine engine, DateTime? expectedDateTime, string inputTemplate, TimeSpan? maximumDelta = null )
        {
            var outputString = GetTemplateOutput( engine, inputTemplate );

            DateTime outputDate;

            var isValidDate = DateTime.TryParse( outputString, out outputDate );

            WriteOutputToDebug( engine, outputString );

            Assert.That.True( isValidDate, $"Template Output does not represent a valid DateTime. [Output=\"{ outputString }\"]" );

            if ( maximumDelta != null )
            {
                DateTimeAssert.AreEqual( expectedDateTime, outputDate, maximumDelta.Value );
            }
            else
            {
                DateTimeAssert.AreEqual( expectedDateTime, outputDate );
            }
        }

        /// <summary>
        /// Resolve the specified template to a date and verify that it is equivalent to the expected date.
        /// </summary>
        /// <param name="expectedDateString"></param>
        /// <param name="inputTemplate"></param>
        /// <param name="maximumDelta"></param>
        public void AssertTemplateOutputDate( string expectedDateString, string inputTemplate, TimeSpan? maximumDelta = null )
        {
            DateTime expectedDate;

            var isValid = DateTime.TryParse( expectedDateString, out expectedDate );

            Assert.That.True( isValid, "Expected Date String input is not a valid date." );

            AssertTemplateOutputDate( expectedDate, inputTemplate, maximumDelta );
        }

        /// <summary>
        /// Write the rendered template to debug output.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="outputString"></param>
        private void WriteOutputToDebug( ILavaEngine engine, string outputString )
        {
            Debug.Print( $"\n**\n** Template Output ({engine.EngineName}):\n**\n{outputString}" );
        }

        #region Test Data

        /// <summary>
        /// Return an initialized Person object for test subject Ted Decker.
        /// </summary>
        /// <returns></returns>
        public TestPerson GetTestPersonTedDecker()
        {
            var campus = new TestCampus { Name = "North Campus", Id = 1 };
            var person = new TestPerson { FirstName = "Edward", NickName = "Ted", LastName = "Decker", Campus = campus, Id = 1 };

            return person;
        }

        /// <summary>
        /// Return an initialized Person object for test subject Alisha Marble.
        /// </summary>
        /// <returns></returns>
        public TestPerson GetTestPersonAlishaMarble()
        {
            var campus = new TestCampus { Name = "South Campus", Id = 102 };
            var person = new TestPerson { FirstName = "Alisha", NickName = "Alisha", LastName = "Marble", Campus = campus, Id = 2 };

            return person;
        }

        /// <summary>
        /// Return an initialized Person object for test subject Alisha Marble.
        /// </summary>
        /// <returns></returns>
        public TestPerson GetTestPersonBillMarble()
        {
            var campus = new TestCampus { Name = "South Campus", Id = 101 };
            var person = new TestPerson { FirstName = "William", NickName = "Bill", LastName = "Marble", Campus = campus, Id = 2 };

            return person;
        }

        /// <summary>
        /// Return a collection of initialized Person objects for the Decker family.
        /// </summary>
        /// <returns></returns>
        public List<TestPerson> GetTestPersonCollectionForDecker()
        {
            var personList = new List<TestPerson>();

            personList.Add( GetTestPersonTedDecker() );
            personList.Add( new TestPerson { FirstName = "Cindy", NickName = "Cindy", LastName = "Decker", Id = 2 } );
            personList.Add( new TestPerson { FirstName = "Noah", NickName = "Noah", LastName = "Decker", Id = 3 } );
            personList.Add( new TestPerson { FirstName = "Alex", NickName = "Alex", LastName = "Decker", Id = 4 } );

            return personList;
        }

        /// <summary>
        /// Return a collection of initialized Person objects for the Decker family.
        /// </summary>
        /// <returns></returns>
        public List<TestPerson> GetTestPersonCollectionForDeckerAndMarble()
        {
            var personList = new List<TestPerson>();

            personList.Add( GetTestPersonTedDecker() );
            personList.Add( new TestPerson { FirstName = "Cindy", NickName = "Cindy", LastName = "Decker", Id = 2 } );
            personList.Add( new TestPerson { FirstName = "Noah", NickName = "Noah", LastName = "Decker", Id = 3 } );
            personList.Add( new TestPerson { FirstName = "Alex", NickName = "Alex", LastName = "Decker", Id = 4 } );

            personList.Add( GetTestPersonBillMarble() );
            personList.Add( GetTestPersonAlishaMarble() );

            return personList;
        }

        /// <summary>
        /// A representation of a Person used for testing purposes.
        /// </summary>
        public class TestPerson : LavaDataObject
        {
            public int Id { get; set; }
            public string NickName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public TestCampus Campus { get; set; }

            public override string ToString()
            {
                return $"{NickName} {LastName}";
            }
        }

        /// <summary>
        /// A representation of a Campus used for testing purposes.
        /// </summary>
        public class TestCampus : LavaDataObject
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// A representation of a Person used for testing purposes.
        /// </summary>
        public class TestSecuredRockDynamicObject : RockDynamic
        {
            [LavaVisible]
            public int Id { get; set; }
            public string NickName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public TestCampus Campus { get; set; }

            public override string ToString()
            {
                return $"{NickName} {LastName}";
            }
        }

        /// <summary>
        /// A representation of a Person used for testing purposes.
        /// </summary>
        public class TestSecuredLavaDataObject : LavaDataObject
        {
            [LavaVisible]
            public int Id { get; set; }
            public string NickName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public TestCampus Campus { get; set; }

            public override string ToString()
            {
                return $"{NickName} {LastName}";
            }
        }

        #endregion

        #region RockLiquid Data Objects

        /// <summary>
        /// Return a collection of initialized Person objects for the Decker family.
        /// </summary>
        /// <returns></returns>
        public List<TestPersonRockLiquid> GetTestPersonCollectionForDeckerRockLiquid()
        {
            var personList = new List<TestPersonRockLiquid>();

            personList.Add( GetTestPersonTedDeckerRockLiquid() );
            personList.Add( new TestPersonRockLiquid { FirstName = "Cindy", NickName = "Cindy", LastName = "Decker", Id = 2 } );
            personList.Add( new TestPersonRockLiquid { FirstName = "Noah", NickName = "Noah", LastName = "Decker", Id = 3 } );
            personList.Add( new TestPersonRockLiquid { FirstName = "Alex", NickName = "Alex", LastName = "Decker", Id = 4 } );

            return personList;
        }


        /// <summary>
        /// Return an initialized Person object for test subject Ted Decker.
        /// </summary>
        /// <returns></returns>
        public TestPersonRockLiquid GetTestPersonTedDeckerRockLiquid()
        {
            var campus = new TestCampusRockLiquid { Name = "North Campus", Id = 1 };
            var person = new TestPersonRockLiquid { FirstName = "Edward", NickName = "Ted", LastName = "Decker", Campus = campus, Id = 1 };

            return person;
        }

        /// <summary>
        /// A representation of a Person used for testing purposes.
        /// </summary>
        public class TestPersonRockLiquid : RockDynamic
        {
            public int Id { get; set; }
            public string NickName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public TestCampusRockLiquid Campus { get; set; }

            public override string ToString()
            {
                return $"{NickName} {LastName}";
            }
        }

        /// <summary>
        /// A representation of a Campus used for testing purposes.
        /// </summary>
        public class TestCampusRockLiquid : RockDynamic
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion
    }
}