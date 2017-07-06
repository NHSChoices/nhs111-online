﻿using System;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.Logging
{
    public class LogEntry : TableEntity
    {
        public LogEntry()
        {
            var now = DateTime.UtcNow;
            PartitionKey = string.Format("{0:yyyy-MM}", now);
            RowKey = string.Format("{0:dd HH:mm:ss.fff}-{1}", now, Guid.NewGuid());
            TIMESTAMP = now;
        }

        [JsonProperty(PropertyName = "TIMESTAMP")]
        public DateTime TIMESTAMP { get; set; }
    }
}
