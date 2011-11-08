﻿using System.Linq;
using NUnit.Framework;
using SisoDb.Dac;
using SisoDb.Querying.Lambdas.Converters.Sql;
using SisoDb.UnitTests.TestFactories;

namespace SisoDb.UnitTests.Querying.Lambdas.Converters.Sql.LambdaToSqlWhereConverterTests
{
    [TestFixture]
    public class LambdaToSqlWhereConverterHierarchyTests : LambdaToSqlWhereConverterTestBase
    {
        [Test]
        public void Process_WhenFirstLevel_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.Int1 == 42);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] {new DacParameter("@p0", 42)};
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void Process_WhenNested_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.NestedItem.Int1 == 42);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "NestedItem.Int1"));
            Assert.AreEqual("(mem0.[IntegerValue] = @p0)", query.CriteriaString);
        }

        [Test]
        public void Process_WhenNested_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.NestedItem.Int1 == 42);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 42) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void Process_WhenDeepNested_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.NestedItem.SuperNestedItem.Int1 == 42);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "NestedItem.SuperNestedItem.Int1"));
            Assert.AreEqual("(mem0.[IntegerValue] = @p0)", query.CriteriaString);
        }

        [Test]
        public void Process_WhenDeepNested_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.NestedItem.SuperNestedItem.Int1 == 42);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 42) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void Process_WhenNestedMemberComparedToFieldValue_GeneratesCorrectSqlQuery()
        {
            var item = new MyItem {NestedItem = new MyNestedItem {Int1 = 42}};
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.NestedItem.Int1 == item.NestedItem.Int1);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "NestedItem.Int1"));
            Assert.AreEqual("(mem0.[IntegerValue] = @p0)", query.CriteriaString);
        }

        [Test]
        public void Process_WhenNestedMemberComparedToFieldValue_ExtractsCorrectParameters()
        {
            var item = new MyItem { NestedItem = new MyNestedItem { Int1 = 42 } };
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.NestedItem.Int1 == item.NestedItem.Int1);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 42) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }
    }
}