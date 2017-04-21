﻿using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dynamo.Api.Tests
{
    [TestClass]
    public class Tests
    {
        private static readonly Uri DynamoUri = new Uri("http://dynamourl.dynamosoftware.com");
        private const string UserName = "test@test.com"; // Tenant specific username goes here
        private const string Password = "test"; // Password
        private const string Tenant = "yourtenant"; // Contact your Dynamo administrator to obtain the tenant name if unknown

        [TestMethod]
        [TestCategory("Login")]
        public void InvalidLogin()
        {
            try
            {
                DynamoV1Client.Login(DynamoUri, "Invalid User Name", "Invalid Password", Tenant);
                Assert.Fail();
            }
            catch (WebException)
            {
            }
        }

        [TestMethod]
        [TestCategory("Login")]
        public void ValidLogin()
        {
            DynamoV1Client.Login(DynamoUri, UserName, Password, Tenant);
        }

        [TestMethod]
        [TestCategory("SearchDocument")]
        public void SearchDocumentNextToken()
        {
            var client = DynamoV1Client.Login(DynamoUri, UserName, Password, Tenant);
            var documentResults =
                client.SearchDocuments("friday OR letter OR event OR fund");

            Assert.IsNotNull(documentResults.NextToken);
            Assert.AreEqual(10, documentResults.Items.Length);
        }

        [TestMethod]
        [TestCategory("SearchDocument")]
        public void SearchDocument()
        {
            var client = DynamoV1Client.Login(DynamoUri, UserName, Password, Tenant);
            SearchDocumentResponseItem[] documentResults =
                client.SearchDocumentsContinuous("friday OR letter OR event OR fund").Take(20).ToArray();

            Assert.AreEqual(13, documentResults.Length);
        }
    }
}
