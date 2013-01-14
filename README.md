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

To exclude the Id field, we first need to create an instance of the PropertiesContractResolver class and add **"Movie.Id"** to the ExcludeProperties property:

```c#
    var propertiesContractResolver = new PropertiesContractResolver();
    propertiesContractResolver.ExcludeProperties.Add("Movie.Id");
```

Then we need to create a JsonSerializerSettings instance and set its ContractResolver to our PropertiesContractResolver instance:
First, you need to set the ContractResolver property of a JsonSerializerSettings instance to an PropertiesContractResolver instance:
```c#
    var serializerSettings = new JsonSerializerSettings();
    serializerSettings.ContractResolver = propertiesContractResolver;
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

## Properties on root type
The most important properties are often the properties on the root type that is to be serialized. For those properties, you do not need to specify the type. Therefore, we can modify our earlier example as follows:
```c#
    propertiesContractResolver.Properties.Add("Title");
    propertiesContractResolver.Properties.Add("Director");

    // which is equivalent to ...
    propertiesContractResolver.Properties.Add("Movie.Title");
    propertiesContractResolver.Properties.Add("Movie.Director");
```

## Wildcards
To make things even easier, it is also possible to use wildcards for property names. For example, if you want to exclude all properties of the Movie class you can also do:

```c#
    var propertiesContractResolver = new PropertiesContractResolver();
    propertiesContractResolver.ExcludeProperties.Add("Movie.*");
```

## Combining properties in a single string
It is possible to specify multiple properties in a single string. The properties in that string must be comma- or space separated. The following two statement blocks are semantically equivalent:

```c#
    // Combined properties version
    propertiesContractResolver.Properties.Add("Title Director");

    // Single properties version
    propertiesContractResolver.Properties.Add("Title");
    propertiesContractResolver.Properties.Add("Director");    
```

## Initializing properties in constructor
To make it more easier to set the properties on an instance of the PropertiesContractResolver class, there are two constructor overloads that immediately set the properties for you:

```c#
    // 1. String constructor overload
    var propertiesContractResolver = new PropertiesContractResolver("Title Director", "Id");

    // which is equivalent to ...
    var propertiesContractResolver = new PropertiesContractResolver();
    propertiesContractResolver.Properties.Add("Title");
    propertiesContractResolver.Properties.Add("Director");
    propertiesContractResolver.ExcludeProperties.Add("Id");

    // 2. String collection constructor overload
    var propertiesContractResolver = new PropertiesContractResolver(new[] { "Title", "Director" }, new string[0]);

    // which is equivalent to ...
    var propertiesContractResolver = new PropertiesContractResolver();
    propertiesContractResolver.Properties.Add("Title");
    propertiesContractResolver.Properties.Add("Director");
```

## Get it on NuGet!
The library is available on NuGet package available. You can install it using the following command:

    Install-Package JsonDotNet.CustomContractResolvers

## History
<table>
  <tr>
     <th>Date</th>
     <th>Version</th>
     <th>Changes</th>
  </tr>
  <tr>
     <td>2013-01-12</td>
     <td>1.1</td>
     <td>
        Properties and exclude properties can be set in the constructor of the PropertiesContractResolver class.
        Properties and exclude properties can be set through comma- and space-separated strings.
        Properties on the root now do not require the root type to be specified.
     </td>
  </tr>
  <tr>
     <td>2013-01-11</td>
     <td>1.0</td>
     <td>None. Initial version.</td>
  </tr>
</table>

## License
[MIT License](https://github.com/ErikSchierboom/JsonDotNetCustomContractResolvers/blob/master/LICENSE.md)