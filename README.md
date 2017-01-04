# RedisStoreWrapper
A basic typed wrapper over Redis to store and search objects using ServiceStack.Redis

### Basic Usage
#### Setup
```csharp
var objects =
    Enumerable.Range(0, 100)
        .Select(i => new MyType {Id = i, Name = $"Object {i}", Comment = $"Comment {i}"})
        .ToList();

var builder = new RedisStoreBuilder<MyType>(
        new SearchTerm<MyType>("Id", o => o.Id.ToString()),
        new RedisEndpoint("hostname", 6380,
            "password")
        {
            Ssl = true
        })
    .AddSearchTerm(new SearchTerm<MyType>("Name", o => o.Name))
    .AddData(objects);

var store = builder.Build();
```
#### Find Items By Primary Key
```csharp
var object1 = store.Find(1);
```
#### Search By Term
```csharp
var objectsBetween50And60 = store.Search("Name", "Object 5");
```
