#ifndef KeychainWrapper_h
#define KeychainWrapper_h

#import <Foundation/Foundation.h>

@interface KeychainWrapper : NSObject

+(BOOL)saveToKeychainWithKey:(NSString*)key data:(NSString*)data;
+(NSString*)getFromKeychainWithKey:(NSString*)key;
+(BOOL)deleteFromKeychainWithKey:(NSString*)key;

@end

#endif /* KeychainWrapper_h */
