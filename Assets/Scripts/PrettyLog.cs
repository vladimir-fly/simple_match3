﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace SM3.Helpers
{
    public class PrettyLog
    {
        public static string GetMessage(string message)
        {
            return SetFontSize($"{message}");
        }

        public static string GetMessage(string @class, string method, string message)
        {
            return SetFontSize($"[{@class}][{method}] {message}");
        }

        public static string GetMessage<T>(string @class, string method, IEnumerable<T> args)
        {
            var j = 0;
            var message = 
                args.Any() ? args.Select(arg => $"[{j++}:{arg}]").Aggregate((arg1, arg2) => $"{arg1}{arg2}") : "Collection is empty!";

            return SetFontSize($"[{@class}][{method}] {message}");
        }

        public static string GetMessage(Action method)
        {
            return SetFontSize($"[{method.Method.DeclaringType}][{method.Method.Name}]");
        }

        public static string GetMessage(Action method, string message)
        {
            return SetFontSize($"[{method.Method.DeclaringType}][{method.Method.Name}] {message}");
        }

        public static string GetMessage<T>(Action<T> method, string message)
        {
            return SetFontSize($"[{method.Method.DeclaringType}][{method.Method.Name}] {message}");
        }

        public static string GetMessage(Action<object[]> method, params object[] args)
        {
            return SetFontSize($"[{method.Method.DeclaringType}][{method.Method.Name}] {args.Aggregate(String.Empty, (x, y) => $"{x} {y}")}");
        }

        public static string GetMessage(MethodInfo methodInfo)
        {
            return SetFontSize($"[{methodInfo.DeclaringType}][{methodInfo.Name}]");
        }

        public static string GetMessage(MethodInfo methodInfo, string message)
        {
            return SetFontSize($"[{methodInfo.DeclaringType}][{methodInfo.Name}] {message}");
        }

        public static string GetMessage(Color color, string message)
        {
            var tmp = (Color32) color;
            return SetFontSize($"<color=#{tmp.r:X2}{tmp.g:X2}{tmp.b:X2}> \u2587 </color>{message}");
        }

        private static string SetFontSize(string message, int fontSize = 14)
        {
            return $"<size={fontSize}>{message}</size>";
        }

        private static string SetFontColor(string message, Color fontColor)
        {
            var tmp = (Color32) fontColor;
            return $"<color=#{tmp.r:X2}{tmp.g:X2}{tmp.b:X2}>{message}</color>";
        }

        private static string SetFontColor(object message, Color fontColor)
        {
            return SetFontColor(message.ToString(), fontColor);
        }
    }
}