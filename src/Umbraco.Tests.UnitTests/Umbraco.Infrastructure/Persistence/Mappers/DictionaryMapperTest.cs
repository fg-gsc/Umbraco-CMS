﻿// Copyright (c) Umbraco.
// See LICENSE for more details.

using NUnit.Framework;
using Umbraco.Cms.Tests.UnitTests.TestHelpers;
using Umbraco.Core.Persistence.Mappers;

namespace Umbraco.Cms.Tests.UnitTests.Umbraco.Infrastructure.Persistence.Mappers
{
    [TestFixture]
    public class DictionaryMapperTest
    {
        [Test]
        public void Can_Map_Id_Property()
        {
            // Act
            string column = new DictionaryMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Id");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsDictionary].[pk]"));
        }

        [Test]
        public void Can_Map_Key_Property()
        {
            // Act
            string column = new DictionaryMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("Key");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsDictionary].[id]"));
        }

        [Test]
        public void Can_Map_ItemKey_Property()
        {
            // Act
            string column = new DictionaryMapper(TestHelper.GetMockSqlContext(), TestHelper.CreateMaps()).Map("ItemKey");

            // Assert
            Assert.That(column, Is.EqualTo("[cmsDictionary].[key]"));
        }
    }
}
