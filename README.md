# C# Routee SDK
 
Routee is a messaging platform. They provided an API for their platform. The documentation can be seen here:
 
http://docs.routee.net/#/documentation

This is its SDK in C#. This sdk contains a sample console app with different use cases which will guide you how you can use this SDK and its various methods. It also contains unit tests, you can go through them as well for better understanding. You just need to provide APP ID and Secret to SDK Manager and then login. You can make next calls easily after logging in. Here is just a sample code.

            SdkManager manager = new SdkManager("APP ID", "Secret");

            //STEP 1 Get Authorized
            RouteResponse<LoginResponse> response = manager.Login();
            
            if (response.HasValue)
            {
                Console.WriteLine("Login Successfullll");
                Console.WriteLine("Access token= " + response.Response.access_token);
                
                //You can make call to any endpoint here using sdk manager object.
            }

            Console.ReadKey();
            
This SDK has a generic class for response, which contains all the required information at its root level.


    namespace Routee.Sdk.Models
    {
        /// <summary>
        /// Route response
        /// </summary>
        /// <typeparam name="T">Type of response</typeparam>
        public class RouteResponse<T>
        {
            /// <summary>
            /// Status code received
            /// </summary>
            public HttpStatusCode StatusCode { get; set; }
    
            /// <summary>
            /// Response recieved, will be null if there are errors
            /// </summary>
            public T Response { get; set; }
    
            /// <summary>
            /// Indicates response has a value
            /// </summary>
            public bool HasValue { get { return Response != null; } }
    
            /// <summary>
            /// Error object
            /// </summary>
            public Error Error { get; set; }
        }
    }

The Error class provides you all the information that you receive from their API in case of an error in a friendly and easy way.

    namespace Routee.Sdk.Models
    {
        /// <summary>
        /// Error
        /// </summary>
        public class Error
        {
            /// <summary>
            /// Error Code received
            /// </summary>
            public string code { get; set; }
    
            /// <summary>
            /// Friendly developers message
            /// </summary>
            public string developerMessage { get; set; }
    
            /// <summary>
            /// Error in the properties sent
            /// </summary>
            public Dictionary<string, string> properties { get; set; }
    
            /// <summary>
            /// Check if there are any properties
            /// </summary>
            public bool HasProperties
            {
                get { return properties != null && properties.Any(); }
            }
        }
    }

