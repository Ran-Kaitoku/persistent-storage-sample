# 説明

## Androidフォルダー
Android Libraryプロジェクトが格納されています。ビルド後、.aarファイルが生成してくれます。
#### 注意：
一応`EncryptedSharedPreferences/SharedPreferences`だけで動作確認してみましたが、アプリをアンインストールした後、`SharedPreferences`に格納されたデータも一緒に消されます。再インストールしても新しい`SharedPreferences`が作成されたので、データを永続的に保存するのは不可になってしまいます。

##### 対応策：
Androidの自動バックアップを有効すると永続的に保存可能になります。ただし、保存場所は`Google Drive`になり、保存先は変更不可です。その他、保存のタイミングもAndroidシステムによって定義された条件の下で動作し、手動で制御できないらしいです。

自動バックアップのタイミングは下記のようです：
- Wi-Fiネットワークに接続されている時；
- デバイスがアイドル状態の時；
- デバイスが充電時

##### 自動バックアップを有効にする実装
- AndroidManifest.xml (Android Library/Unityプロジェクト両方のいずれで変更可)
  - `application`に`android:autoBackup`と`android:fullBackupContent="@xml/backup_descriptor"`を追加：
    ```xml
        <application android:allowBackup="true"
            android:fullBackupContent="@xml/backup_descriptor">
    ```
    fullBackupContentは自動バックアップのルールを規制するxmlファイルを指定する；
  
  - `res/xml/`フォルダの直下に`backup_descriptor.xml`の作成：
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <full-backup-content>
        <include domain="sharedpref" path="." />
    </full-backup-content>
    ```
  このxmlは`SharedPreferences`に格納されいたデータをバックアップすることを指定しています；

このAndroidサンプルコードをAndroid Libraryプロジェクトとしてビルドすると、`.aar`ファイルが作成してくれます。上記の`AndroidManifest.xml`と`backup_descriptor.xml`も`.aar`ファイルに含まれる。

Note: AndroidManifest.xmlをUnity側で変更する場合、このファイルをAndroid LibraryプロジェクトからUnityのPlugins/Androidに移動してください。  
ちなみに、直近のUnityのバージョンだと、Plugins/Android/resフォルダーのビルドをサポートしなくなりますので、backup_descriptor.xmlをUnityに移動するとコンパイルエラーが出てしまいます。

ビルド後の`.aar`ファイルをUnityプロジェクトの`Plugins/Android`の直下に移動するだけで済みます。

## iOSフォルダー
iOSフォルダーの直下の`.h`ファイルと`.m`ファイルをUnityの`Plugins/iOS`の直下に移動するだけで済みます。

## Unityフォルダー
Unityフォルダーの直下にサンプルのUnityプロジェクトが格納されています。`Plugins`フォルダーと`Scripts`フォルダー直下のコードを確認すれば実装の内容がすぐわかると思いますが、ちょっと注意しないとならない点を簡単に説明します。

```csharp
    private static readonly string _className = "jp.smartshare.shared_preferences.SharedPreferencesManager";
```
これはAndroidのクラスのフルパスです。`jp.smartshare.shared_preferences`はパッケージ名で、`SharedPreferencesManager`はクラス名です。Android Libraryサンプルコードを変更する場合、こちらの編集も必要です。

```csharp
    private static readonly string _unityClassName = "com.unity3d.player.UnityPlayer";
    private static readonly string _androidActivityName = "currentActivity";
```
これはUnityプロジェクトがAndroidアプリにビルドされる時UnityのクラスのフルパスとActivity名です。**変更しない**でください。

```csharp
    javaClass.CallStatic("saveString", context, key, value, needsEncrypted ? 1 : 0);
```
UnityのboolタイプがAndroidのBooleanに自動変換できないらしいので、Unity側でBoolをIntに変換してAndroidに渡す必要がありそうです。

その他の質問がありましたら、お気軽にご聞きください。