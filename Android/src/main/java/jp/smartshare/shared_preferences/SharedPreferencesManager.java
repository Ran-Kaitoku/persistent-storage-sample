package jp.smartshare.shared_preferences;

import android.content.Context;
import android.content.SharedPreferences;

import androidx.security.crypto.EncryptedSharedPreferences;
import androidx.security.crypto.MasterKey;

import java.io.IOException;
import java.security.GeneralSecurityException;

public class SharedPreferencesManager {
    private static SharedPreferences getUnencryptedSharedPreferences(Context context) {
        return context.getSharedPreferences("SmartSharePreferences", Context.MODE_PRIVATE);
    }

    private static SharedPreferences getEncryptedSharedPreferences(Context context) {
        try {
            MasterKey masterKey = new MasterKey.Builder(context).setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
                    .build();
            return EncryptedSharedPreferences.create(context, "SmartSharePreferences", masterKey,
                    EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
                    EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM);
        } catch (GeneralSecurityException | IOException e) {
            throw new RuntimeException("Failed to initialize EncryptedSharedPreferences");
        }
    }

    private static SharedPreferences getSharedPreferences(Context context, Boolean encrypted) {
        if (encrypted) {
            return getEncryptedSharedPreferences(context);
        }
        return getUnencryptedSharedPreferences(context);
    }
    public static void saveString(Context context, String key, String value, int needsEncryption) {
        SharedPreferences.Editor editor = getSharedPreferences(context, needsEncryption == 1).edit();
        editor.putString(key, value);
        editor.apply();
    }

    public static String getString(Context context, String key, String defaultValue, int isEncrypted) {
        return getSharedPreferences(context, isEncrypted == 1).getString(key, defaultValue);
    }

    public static boolean remove(Context context, String key, int isEncrypted) {
        SharedPreferences.Editor editor = getSharedPreferences(context, isEncrypted == 1).edit();
        editor.remove(key);
        return editor.commit();
    }
}
