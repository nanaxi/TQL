//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: ProtoBuffer/ClientAndGCCmd.proto
namespace ProtoBuf
{
    [global::ProtoBuf.ProtoContract(Name=@"CLIToGCProtocol")]
    public enum CLIToGCProtocol
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"CLI_TO_GC_PROTOCOL_BEGIN", Value=1)]
      CLI_TO_GC_PROTOCOL_BEGIN = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CLI_TO_GC_PROTOCOL_COUNT", Value=1023)]
      CLI_TO_GC_PROTOCOL_COUNT = 1023,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CLI_TO_GC_PROTOCOL_END", Value=1024)]
      CLI_TO_GC_PROTOCOL_END = 1024
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"GCToCLIProtocol")]
    public enum GCToCLIProtocol
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"GC_TO_CLI_PROTOCOL_BEGIN", Value=1)]
      GC_TO_CLI_PROTOCOL_BEGIN = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GC_TO_CLI_PROTOCOL_COUNT", Value=1023)]
      GC_TO_CLI_PROTOCOL_COUNT = 1023,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GC_TO_CLI_PROTOCOL_END", Value=1024)]
      GC_TO_CLI_PROTOCOL_END = 1024
    }
  
}