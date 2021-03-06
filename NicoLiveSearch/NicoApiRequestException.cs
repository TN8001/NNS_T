﻿using System;
using System.Net.Http;

namespace NicoLiveSearch
{
    ///<summary>niconicoコンテンツ検索API 取得エラー</summary>
    public class NicoApiRequestException : HttpRequestException
    {
        public NicoApiRequestException(string message) : base(message) { }
        public NicoApiRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
