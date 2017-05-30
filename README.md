# SightReader

## Development

### MusicXML Schema Generation

So you've got MusicXML files in the form of `my-music-file.xml`, and you want to parse it.

The first task is to deserialize the XML into something our program can understand, like a set of strongly typed C# classes that correspond to the MusicXML schema. While there are [libraries out there](https://github.com/vdaron/MusicXml.Net) that can parse MusicXML into C# classes, there are easier solutions.

The MusicXML XSD schema, provided by the authors of the MusicXML spec, can be used to automatically generate the corresponding C# classes for easy deserialization. The great thing about automatic generation is the guaranteed correctness and ease of maintenance. I don't expect the MusicXML 3.0 standard to change to 4.0 soon, but if it does, it's trivial to generate a strongly typed class equivalent.

I used Microsoft's built-in tool `xsd.exe` to generate C# classes automatically. The command used was:

```
xsd musicxml.xsd opus.xsd xlink.xsd xml.xsd /classes /language:CS /namespace:Engine.Builder.MusicXml /fields
```

I tried more popular tools like Xsd2Code, but it did not handle multiple files (MusicXML's schema is defined across 4 XSD files), and the pascal casing it promised only uppercased the first letter. I settled for xsd.exe (the only working tool) even though xsd.exe doesn't support pascal-casing class/field names. I was surprised to find few results for XML to C# classes generators, given the historic popularity of XML. I'm also surprised how few options Microsoft's `xsd.exe` tool has considering how long it's been released and used.