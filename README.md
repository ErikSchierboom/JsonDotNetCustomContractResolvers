# JSON.NET custom contract resolvers
Libraries containing custom contract resolvers for JSON.NET.

## What can it be used for?
The PropertiesContractResolver class can be used to easily serialize only specific fields, which can be very convenient when building an API that allows the user to specify what fields are to be returned. It also allows fields to be excluded from serialization.

## Usage
Let's say that we will be serializing an instance of the following class:
```c#
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
    }
```

If we serialize an instance of this class using JSON.NET and the default settings, the output will look like this:
```json
    {"Id":12,"Title":"Inception","Director":"Christopher Nolan"}
```
Now suppose that we do not want to serialize the Id property. We can do this by plugging-in the PropertiesContractResolver. The PropertiesContractResolver has a property named ExcludeProperties that contains a set of property names that are to be excluded from serialization. The property names have the format: **"{DeclaringTypeName}.{PropertyName}"**. In our example, the Id field's property name is: **"Movie.Id"**.

To exclude the Id field, we first need to create an instance of the PropertiesContractResolver class and add **"Movie.Id"** to the ExcludedProperties property:

```c#
    var propertiesContractResolver = new PropertiesContractResolver();
    propertiesContractResolver.ExcludedProperties.Add("Movie.Id");
```

Then we need to create a JsonSerializerSettings instance and set its ContractResolver to our PropertiesContractResolver instance:
First, you need to set the ContractResolver property of a JsonSerializerSettings instance to an PropertiesContractResolver instance:
```c#
    var serializerSettings = new JsonSerializerSettings { ContractResolver = propertiesContractResolver };
```

Now, we are ready to serialize our Movie instance again, but now using our PropertiesContractResolver instance that ignores the Id property. We do this by having JSON.NET use our customized JsonSerializerSettings instance when serializing:

```c#
    JsonConvert.SerializeObject(movie, serializerSettings);
```
The value returned will now equal:
```json
    {"Title":"Inception","Director":"Christopher Nolan"}
```
Of course, we could have also approached this the other way around by specifying which properties to return:

```c#
    var propertiesContractResolver = new PropertiesContractResolver();
    propertiesContractResolver.Properties.Add("Movie.Title");
    propertiesContractResolver.Properties.Add("Movie.Director");
```

This will reult in the same output:

```json
    {"Title":"Inception","Director":"Christopher Nolan"}
```

You can also use a combination of properties to return and properties to exclude.

## Wildcards
To make things even easier, it is also possible to use wildcards for property names. For example, if you want to exclude all properties of the Movie class you can also do:

```c#
    var propertiesContractResolver = new PropertiesContractResolver();
    propertiesContractResolver.ExcludedProperties.Add("Movie.*");
```

## Get it on NuGet!
The library is available on NuGet package available. You can install it using the following command:

    Install-Package JsonDotNet.CustomContractResolvers
	
## License
[MIT License](https://github.com/ErikSchierboom/JsonDotNetCustomContractResolvers/blob/master/LICENSE.md)