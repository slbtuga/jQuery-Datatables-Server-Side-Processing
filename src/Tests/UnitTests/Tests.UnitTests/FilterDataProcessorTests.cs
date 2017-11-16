﻿namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Models;

    public class FilterDataProcessorTests
    {
        [Test]
        public void SearchWithTwoSearchableProperties()
        {
            var filterProc = new FilterDataProcessor();
            var data = new List<SimpleModel>().AsQueryable();
            var processedData = filterProc.OnProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = "aaa"
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Searchable = true
                        },
                        new Column
                        {
                            Data = "Integer",
                            Searchable = true
                        }
                    }
                }
            });

            var expression = ((System.Linq.IQueryable)processedData).Expression;
            var actualExpressionStr = expression.ToString();
            var expectedExpressionStr = "System.Collections.Generic.List`1[Tests.UnitTests.Models.SimpleModel].Where(model => (Convert(model).String.ToString().ToLower().Contains(\"aaa\") Or Convert(model).Integer.ToString().ToLower().Contains(\"aaa\")))";

            Assert.AreEqual(expectedExpressionStr, actualExpressionStr);
        }

        [Test]
        public void SearchWithSingleSearchableProperty()
        {
            var filterProc = new FilterDataProcessor();
            var data = new List<SimpleModel>().AsQueryable();
            var processedData = filterProc.OnProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = "aaa"
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Searchable = true
                        }
                    }
                }
            });

            var expression = ((System.Linq.IQueryable)processedData).Expression;
            var actualExpressionStr = expression.ToString();
            var expectedExpressionStr = "System.Collections.Generic.List`1[Tests.UnitTests.Models.SimpleModel].Where(model => Convert(model).String.ToString().ToLower().Contains(\"aaa\"))";

            Assert.AreEqual(expectedExpressionStr, actualExpressionStr);
        }

        [Test]
        public void ShouldThrowAppropriateExceptionIfSearchValueWithNoSearchableProperties()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var filterProc = new FilterDataProcessor();
                var data = new List<SimpleModel>().AsQueryable();
                var processedData = filterProc.OnProcessData(data, new RequestInfoModel()
                {
                    Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                    TableParameters = new DataTableAjaxPostModel
                    {
                        Search = new Search
                        {
                            Value = "aaa"
                        },
                        Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Searchable = false
                        }
                    }
                    }
                });
            });

            Assert.IsTrue(exception.Message.ToLower().Contains("no searchable properties"));
        }
    }
}