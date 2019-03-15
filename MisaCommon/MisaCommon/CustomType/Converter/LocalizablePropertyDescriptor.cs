namespace MisaCommon.CustomType.Converter
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    using MisaCommon.CustomType.Attribute;

    /// <summary>
    /// ローカライズに対応した <see cref="PropertyDescriptor"/> の派生クラス
    /// </summary>
    /// <typeparam name="TResources">リソースクラスを指定</typeparam>
    internal class LocalizablePropertyDescriptor<TResources> : PropertyDescriptor
    {
        #region クラス変数・定数

        /// <summary>
        /// 基底クラスのインスタンス
        /// </summary>
        private readonly PropertyDescriptor _originalData;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数のインスタンスを使用して初期化する、また引数に指定されたインスタンスは保持する
        /// </summary>
        /// <param name="descriptor">基底クラスのインスタンス</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="descriptor"/> がNULLの場合に発生
        /// </exception>
        public LocalizablePropertyDescriptor(PropertyDescriptor descriptor)
            : base(descriptor)
        {
            _originalData = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// プロパティが属するカテゴリの名前を取得する
        /// </summary>
        /// <exception cref="AmbiguousMatchException">
        /// 取得キーに対して複数の値が存在した場合（キー重複の場合）に発生
        /// </exception>
        public override string Category
        {
            get
            {
                // リソースから取得するためのキーを生成
                string baseKey = string.IsNullOrEmpty(base.Category) ? string.Empty : base.Category;

                // カテゴリ用のソート対応のため、先頭をTrimした値をキーにする
                string key = baseKey.TrimStart();

                // Trimで削除された文字を取得する
                string sortKey = baseKey.Substring(0, baseKey.Length - key.Length);

                // キーをもとにリソースから値を取得
                string value = CommonLocalizable.GetResourceValue(typeof(TResources), key);

                // 値が取得できない場合はカテゴリをそのまま返却
                // 取得できた場合は取得した値にソートキーを追加して返却
                return sortKey + (value ?? baseKey);
            }
        }

        /// <summary>
        /// プロパティの説明を取得する
        /// </summary>
        /// <exception cref="AmbiguousMatchException">
        /// 取得キーに対して複数の値が存在した場合（キー重複の場合）に発生
        /// </exception>
        public override string Description
        {
            get
            {
                // リソースから取得するためのキーを生成
                // 説明が設定されていない場合は「名称＋"_Description"」をキーにする
                // 設定されている場合は、その値をキーにする
                string key = string.IsNullOrEmpty(base.Description) ?
                    Name + "_Description" : base.Description;

                // キーをもとにリソースから値を取得
                string value = CommonLocalizable.GetResourceValue(typeof(TResources), key);

                // 値が取得できない場合は説明をそのまま返却、取得できた場合は取得した値を返却
                return value ?? base.Description;
            }
        }

        /// <summary>
        /// プロパティウィンドウなどのウィンドウに表示する名前を取得する
        /// </summary>
        /// <exception cref="AmbiguousMatchException">
        /// 取得キーに対して複数の値が存在した場合（キー重複の場合）に発生
        /// </exception>
        public override string DisplayName
        {
            get
            {
                // リソースから取得するためのキーを生成
                string baseKey = string.IsNullOrEmpty(base.DisplayName) ? string.Empty : base.DisplayName;

                // メンバー名と表示名が同じ場合は「名称＋"_DisplayName"」をキーにする
                // 異なる場合は表示名の値をキーにする
                string key = baseKey == Name ? Name + "_DisplayName" : baseKey;

                // キーをもとにリソースから値を取得
                string value = CommonLocalizable.GetResourceValue(typeof(TResources), key);

                // 値が取得できない場合は表示名をそのまま返却、取得できた場合は取得した値を返却
                return value ?? base.DisplayName;
            }
        }

        #endregion

        #region 継承元のPropertyDescriptorクラスの抽象メンバーの実装

        /// <summary>
        /// プロパティが関連付けられているコンポーネントの型
        /// </summary>
        public override Type ComponentType => _originalData.ComponentType;

        /// <summary>
        /// プロパティが読み取り専用かどうか
        /// </summary>
        public override bool IsReadOnly => _originalData.IsReadOnly;

        /// <summary>
        /// プロパティの型
        /// </summary>
        public override Type PropertyType => _originalData.PropertyType;

        /// <summary>
        /// オブジェクトをリセットしたときに、そのオブジェクトの値が変化するかどうかを示す値を返す
        /// </summary>
        /// <param name="component">リセット機能について調べる対象のコンポーネントオブジェクト</param>
        /// <returns>
        /// コンポーネントをリセットするとコンポーネントの値が変化する場合は True、それ以外の場合は False
        /// </returns>
        public override bool CanResetValue(object component) => _originalData.CanResetValue(component);

        /// <summary>
        /// コンポーネントのプロパティの現在の値を取得
        /// </summary>
        /// <param name="component">値の取得対象であるプロパティを持つコンポーネントオブジェクト</param>
        /// <returns>指定したコンポーネントのプロパティの値</returns>
        public override object GetValue(object component) => _originalData.GetValue(component);

        /// <summary>
        /// コンポーネントのプロパティの値を既定値にリセットする
        /// </summary>
        /// <param name="component">既定値にリセットする対象のプロパティ値を持つコンポーネントオブジェクト</param>
        public override void ResetValue(object component) => _originalData.ResetValue(component);

        /// <summary>
        /// コンポーネントの値を別の値に設定する
        /// </summary>
        /// <param name="component">設定する対象のプロパティ値を持つコンポーネントオブジェクト</param>
        /// <param name="value">設定する値</param>
        public override void SetValue(object component, object value) => _originalData.SetValue(component, value);

        /// <summary>
        /// プロパティの値を永続化する必要があるかどうかを示す値を決定する
        /// </summary>
        /// <param name="component">永続性について調べる対象のプロパティを持つコンポーネントオブジェクト</param>
        /// <returns>
        /// プロパティを永続化する必要がある場合は True、それ以外の場合は False
        /// </returns>
        public override bool ShouldSerializeValue(object component) => _originalData.ShouldSerializeValue(component);

        #endregion
    }
}
