# Hi.Mapper

`TR` "Hi.Mapper" 

## Installation

Install the package with [NuGet]

    Install-Package HiMapper

## Usage


```C#
var newObject = Mapper.Map<NewMyType>(source);
```

```C#
var newObject = source.Map<NewMyType>();
```

```C#
Mapper.Map(source, target);
```

mapper without some properties (Sample: Password, Id)
```C#
Mapper.Map(source, target, x => new { x.Id, x.Password });
```
```C#
Mapper.Map(source, target, new List<string> { "Id", "Password" });
```

