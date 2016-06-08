﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using OSCforPCL.Values;
using System.IO;

namespace test
{
    public class OSCIntTester
    {
        [InlineData(0)]
        [InlineData(Int32.MaxValue)]
        [InlineData(Int32.MinValue)]
        [Theory]
        public void TestParsing(Int32 value)
        {
            Assert.Equal(value, OSCValueTester.TestOSCValueParser(value, (reader) => OSCInt.Parse(reader)));
        }
    }
}
