﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MisaCommon.MessageResources {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessage() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MisaCommon.MessageResources.ErrorMessage", typeof(ErrorMessage).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   指定したChromeのEXEのパスが間違っておりChromeの起動ができません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ChromeSpeechRecognitionMessageChromePathIsWrong {
            get {
                return ResourceManager.GetString("ChromeSpeechRecognitionMessageChromePathIsWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   当機能を使用するにはChromeのインストールが必要です。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ChromeSpeechRecognitionMessageNoInstall {
            get {
                return ResourceManager.GetString("ChromeSpeechRecognitionMessageNoInstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   既にアプリケーションは起動しています。
        ///二重起動はできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DoubleStartupImpossibleMessage {
            get {
                return ResourceManager.GetString("DoubleStartupImpossibleMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0}
        ///
        ///【エラー内容】
        ///{1} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrorMessageFormat {
            get {
                return ResourceManager.GetString("ErrorMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   エラーが発生しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrorMessageFormatCommon {
            get {
                return ResourceManager.GetString("ErrorMessageFormatCommon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   処理[{0}]にてエラーが発生しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrorMessageFormatCommonWithProcessName {
            get {
                return ResourceManager.GetString("ErrorMessageFormatCommonWithProcessName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   
        ///
        ///--- 【エラー内容】--------------------------------
        ///{0}
        ///-- 【スタックトレース】 --------------------------
        ///{1} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrorMessageFormatWithStackTrace {
            get {
                return ResourceManager.GetString("ErrorMessageFormatWithStackTrace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数はGifの単一のフレームデータとして不正なデータです。（不正なブロックーデータ） に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string GifEncoderErrorBadBlock {
            get {
                return ResourceManager.GetString("GifEncoderErrorBadBlock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数はGifの単一のフレームデータとして不正なデータです。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string GifEncoderErrorBadData {
            get {
                return ResourceManager.GetString("GifEncoderErrorBadData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数はGifの単一のフレームデータとして不正なデータです。（不正なヘッダーデータ） に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string GifEncoderErrorBadHeader {
            get {
                return ResourceManager.GetString("GifEncoderErrorBadHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数はGifの単一のフレームデータとして不正なデータです。（不正な末端データ） に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string GifEncoderErrorBadLast {
            get {
                return ResourceManager.GetString("GifEncoderErrorBadLast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Gifのエンコードに失敗しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string GifEncoderErrorEncodingFailed {
            get {
                return ResourceManager.GetString("GifEncoderErrorEncodingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数はGifの単一のフレームデータとして不正なデータです。（ブロックーデータ無） に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string GifEncoderErrorNoBlock {
            get {
                return ResourceManager.GetString("GifEncoderErrorNoBlock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数にはGifの複数フレームのデータが指定されています。単一のフレームデータである必要があります。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string GifEncoderErrorNotSingleFrame {
            get {
                return ResourceManager.GetString("GifEncoderErrorNotSingleFrame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ローカルHTTPサーバでエラーが発生しました。
        ///処理を終了します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LocalHttpServerError {
            get {
                return ResourceManager.GetString("LocalHttpServerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ローカルHTTPサーバでエラーが発生しました。（エラーコード：{0}）
        ///処理を終了します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LocalHttpServerErrorHttpListenerErrorFormat {
            get {
                return ResourceManager.GetString("LocalHttpServerErrorHttpListenerErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ローカルHTTPサーバの起動に失敗しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LocalHttpServerErrorNotStart {
            get {
                return ResourceManager.GetString("LocalHttpServerErrorNotStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ローカルHTTPサーバの起動に失敗しました。（エラーコード：{0}） に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LocalHttpServerErrorNotStartHttpListenerErrorFormat {
            get {
                return ResourceManager.GetString("LocalHttpServerErrorNotStartHttpListenerErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   このOSではローカルHTTPサーバを使用することはできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LocalHttpServerErrorNotSupported {
            get {
                return ResourceManager.GetString("LocalHttpServerErrorNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   使用可能なポートが存在しないため、ローカルHTTPサーバを使用することはできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LocalHttpServerErrorNoUnusedPort {
            get {
                return ResourceManager.GetString("LocalHttpServerErrorNoUnusedPort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   &lt;!DOCTYPE html&gt;&lt;html&gt;
        ///&lt;head&gt;&lt;meta charset = \&quot;UTF-8\&quot;&gt;&lt;/head&gt;
        ///&lt;body&gt;&lt;pre&gt;
        ///{0}
        ///&lt;/pre&gt;&lt;/body&gt;
        ///&lt;/html&gt; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LocalHttpServerErrorResponseHtmlFormat {
            get {
                return ResourceManager.GetString("LocalHttpServerErrorResponseHtmlFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   注意 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TitleAttention {
            get {
                return ResourceManager.GetString("TitleAttention", resourceCulture);
            }
        }
        
        /// <summary>
        ///   確認 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TitleConfirm {
            get {
                return ResourceManager.GetString("TitleConfirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   致命的なエラー発生！？ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TitleCriticalError {
            get {
                return ResourceManager.GetString("TitleCriticalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   エラー発生！ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TitleError {
            get {
                return ResourceManager.GetString("TitleError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   情報 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TitleInfo {
            get {
                return ResourceManager.GetString("TitleInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ワーニング！ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TitleWarning {
            get {
                return ResourceManager.GetString("TitleWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   コンフィグの読み込みに失敗しました。アプリケーションを終了します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UserSettingsProviderErrorGetProperty {
            get {
                return ResourceManager.GetString("UserSettingsProviderErrorGetProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   コンフィグの読み込み（初期化）に失敗しました。アプリケーションを終了します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UserSettingsProviderErrorInitialize {
            get {
                return ResourceManager.GetString("UserSettingsProviderErrorInitialize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   コンフィグのリセットに失敗しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UserSettingsProviderErrorReset {
            get {
                return ResourceManager.GetString("UserSettingsProviderErrorReset", resourceCulture);
            }
        }
        
        /// <summary>
        ///   コンフィグの書き込みに失敗しました。設定した内容は破棄されます。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string UserSettingsProviderErrorSetProperty {
            get {
                return ResourceManager.GetString("UserSettingsProviderErrorSetProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   「{0}、{1}」が読み込みでエラーが発生しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Win32OperateErrorFailDllImportFormat {
            get {
                return ResourceManager.GetString("Win32OperateErrorFailDllImportFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   「{0}、{1}」の処理に失敗しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Win32OperateErrorFormat {
            get {
                return ResourceManager.GetString("Win32OperateErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   「{0}、{1}」の処理に失敗しました。
        ///{2} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Win32OperateErrorFormatWithErrorCode {
            get {
                return ResourceManager.GetString("Win32OperateErrorFormatWithErrorCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   「{0}、{1}」の処理において例外「RuntimeWrappedException」が発生しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Win32OperateErrorRuntimeWrappedExceptionFormat {
            get {
                return ResourceManager.GetString("Win32OperateErrorRuntimeWrappedExceptionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   タイムアウトが発生しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Win32OperateErrorTimeout {
            get {
                return ResourceManager.GetString("Win32OperateErrorTimeout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   ウィンドウキャプチャの処理でエラーが発生しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string WindowCaptureError {
            get {
                return ResourceManager.GetString("WindowCaptureError", resourceCulture);
            }
        }
    }
}
