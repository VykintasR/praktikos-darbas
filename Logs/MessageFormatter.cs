﻿using System.Net;
using Bezdzione.Request;

namespace Bezdzione.Logs
{
    public static class MessageFormatter
    {
        public static string Info(string message)
        {
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} INFO: {message}{Environment.NewLine}";
        }
        public static string Error(string message)
        {
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} ERROR: {message}{Environment.NewLine}";
        }

        public static string RequestInfo(RequestParameters parameters)
        {
            return $"Trying to deploy server with region: {parameters.RegionSlug}, plan: {parameters.PlanSlug}, image: {parameters.ImageSlug}";
        }

        public static string ResponseInfo(dynamic response, HttpStatusCode status)
        {
            string message = $"Response for Server with Status Code: {status}";
            message += $", Server ID: {response.id}";
            message += $", Server Name: {response.hostname}";
            message += $", Region: {response.region.slug}";
            message += $", Plan: {response.name}";
            message += $", OS Image: {response.image}{Environment.NewLine}";
            return message;
        }
    }
}
