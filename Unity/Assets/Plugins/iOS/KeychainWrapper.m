#import <Security/Security.h>
#import "KeychainWrapper.h"

@implementation KeychainWrapper

+(BOOL)saveToKeychainWithKey:(NSString *)key data:(NSString *)data {
    NSData* dataToStore = [data dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary* query = @{(__bridge id)kSecClass: (__bridge id)kSecClassGenericPassword,
                            (__bridge id)kSecAttrAccount: key,
                            (__bridge id)kSecValueData: dataToStore};
    SecItemDelete((__bridge CFDictionaryRef)query);
    OSStatus status = SecItemAdd((__bridge CFDictionaryRef)query, NULL);
    
    return status == errSecSuccess;
}

+(NSString*)getFromKeychainWithKey:(NSString *)key {
    NSDictionary* query = @{(__bridge id)kSecClass: (__bridge id)kSecClassGenericPassword,
                            (__bridge id)kSecAttrAccount: key,
                            (__bridge id)kSecReturnData: @YES,
                            (__bridge id)kSecMatchLimit: (__bridge id)kSecMatchLimitOne};
    CFTypeRef dataTypeRef = NULL;
    OSStatus status = SecItemCopyMatching((__bridge CFDictionaryRef)query, &dataTypeRef);
    
    if (status == errSecSuccess) {
        NSData* retrievedData = (__bridge_transfer NSData*)dataTypeRef;
        NSString* result = [[NSString alloc]initWithData:retrievedData encoding:NSUTF8StringEncoding];
        return result;
    }
    
    return nil;
}

+(BOOL)deleteFromKeychainWithKey:(NSString *)key {
    NSDictionary* query = @{(__bridge id)kSecClass: (__bridge id)kSecClassGenericPassword,
                            (__bridge id)kSecAttrAccount: key};
    OSStatus status = SecItemDelete((__bridge CFDictionaryRef)query);
    return status == errSecSuccess;
}

@end

#ifdef __cplusplus
extern "C" {
#endif
    
    bool saveToKeychainWithKey(const char *key, const char *data) {
        return [KeychainWrapper saveToKeychainWithKey:[NSString stringWithUTF8String:key] data:[NSString stringWithUTF8String:data]];
    }
    
    const char* getFromKeychainWithKey(const char *key) {
        NSString *result = [KeychainWrapper getFromKeychainWithKey:[NSString stringWithUTF8String:key]];
        if (result) {
            // You need to ensure the string returned is copied to the C heap, so Unity can free it later
            // Alternatively, manage memory manually, but be cautious of memory leaks
            return strdup([result UTF8String]);
        }
        return NULL;
    }
    
    bool deleteFromKeychainWithKey(const char *key) {
        return [KeychainWrapper deleteFromKeychainWithKey:[NSString stringWithUTF8String:key]];
    }
    
#ifdef __cplusplus
}
#endif
