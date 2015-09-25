# JSON.NET custom contract resolvers
Libraries containing custom contract resolvers for JSON.NET.

[![Build status](https://ci.appveyor.com/api/projects/status/1duqfo4432r3ka1e)](https://ci.appveyor.com/project/ErikSchierboom/jsondotnetcustomcontractresolvers) [![Coverage Status](https://coveralls.io/repos/ErikSchierboom/JsonDotNetCustomContractResolvers/badge.svg?branch=&service=github)](https://coveralls.io/github/ErikSchierboom/JsonDotNetCustomContractResolvers?branch=master)

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

Now suppose that we do not want to serialize the Id property. We can do this by plugging-in the PropertiesContractResolver. The PropertiesContractResolver has a property named ExcludeProperties that contains a set of property names that are to be excluded from serialization. 

To exclude the Id field, we first need to create an instance of the PropertiesContractResolver class and add **"Id"** to the ExcludeProperties property:

```c#
var propertiesContractResolver = new PropertiesContractResolver();
propertiesContractResolver.ExcludeProperties.Add("Id");
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
propertiesContractResolver.Properties.Add("Title");
propertiesContractResolver.Properties.Add("Director");
```

This will reult in the same output:

```json
{"Title":"Inception","Director":"Christopher Nolan"}
``` 

You can also use a combination of properties to return and properties to exclude. The way this works is that first the properties to be returned are filtered, and then we see which of those properties needs to be excluded.

## Property match mode
The previous example only looked at the property name to see if a property matched. However, it is also possible to have properties only match when their combination of property name and type matches. This allows you to be even more specific on what properties to include or exclude. To use this explicit type matching, you have to set the `PropertyMatchMode` property to `PropertyMatchMode.NameAndType` (the default is `PropertyMatchMode.Name`):

```c#
var propertiesContractResolver = new PropertiesContractResolver();
propertiesContractResolver.PropertyMatchMode = PropertyMatchMode.NameAndType;
```

Now properties will only match when both the property name and type are equal to the specified properties. Of course, this also means that you have to specify the type when adding properties. Explicit property names have the format: **"{DeclaringTypeName}.{PropertyName}"**. 

We can see how this works, by returning to our previous example where we want to exclude the Id property. If we do not change anything, the property will not be excluded as we did not specify a type:

```c#
propertiesContractResolver.ExcludeProperties.Add("Id");
```

This will serialize to:
```json
{"Id":12,"Title":"Inception","Director":"Christopher Nolan"}
```

However, if we add the property type, everything works fine:

```c#
propertiesContractResolver.ExcludeProperties.Add("Movie.Id");
```

And this will serialize to: 

```json
{"Title":"Inception","Director":"Christopher Nolan"}
```

## Wildcards
To make things even easier, it is also possible to use wildcards for property names. For example, if you want to exclude all properties of the Movie class you can also do:

```c#
var propertiesContractResolver = new PropertiesContractResolver();
propertiesContractResolver.ExcludeProperties.Add("Movie.*");
```

You can also use a wildcard for property types For example, if you want to return the Id property of all types you can do:

```c#
var propertiesContractResolver = new PropertiesContractResolver();
propertiesContractResolver.Properties.Add("*.Id");
```

Finally, there is also a wildcard that matches all properties. This way you can exclude all properties as follows:

```c#
var propertiesContractResolver = new PropertiesContractResolver();
propertiesContractResolver.Properties.Add("*");
```

## Combining properties in a single string
It is possible to specify multiple properties in a single string. The properties in that string must be comma-, space or tab-separated. The following two statement blocks are semantically equivalent:

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

## Serializing properties collection
The properties collection class has its `ToString` method overloaded to return the properties in a string format. This can be convenient when you want to store the serialized properties. An short example will show how this works:

```c#
// Create the properties collection
var properties = new PropertiesCollection();

// then add the properties
properties.Add("Title");
properties.Add("Director");

// then convert the properties to their string format
var propertiesAsString = properties.ToString(); // Returns "Title,Director"

// we can now create a new properties collection 
var propertiesFromString = new PropertiesCollection(propertiesAsString);
propertiesFromString.SetEquals(properties); // This returns true
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
     <td>2013-01-29</td>
     <td>1.1.2</td>
     <td>
        Added option to specify property match mode (name only matching or property name and type matching).<br/>
        ToString on PropertiesCollection returns properties in format that can also be used as input for PropertiesCollection.<br/>
        Allow properties to be separated by tabs.
     </td>
  </tr>
  <tr>
     <td>2013-01-15</td>
     <td>1.1.1</td>
     <td>
        Added support for type wildcards.
     </td>
  </tr>
  <tr>
     <td>2013-01-12</td>
     <td>1.1.0</td>
     <td>
        Properties and exclude properties can be set in the constructor of the PropertiesContractResolver class.<br/>
        Properties and exclude properties can be set through comma- and space-separated strings.<br/>
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
[Apache License 2.0](LICENSE.md)
