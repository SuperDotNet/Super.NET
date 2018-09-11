``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4), VM=Hyper-V
Unknown processor
.NET Core SDK=2.1.401
  [Host] : .NET Core 2.1.3 (CoreCLR 4.6.26725.06, CoreFX 4.6.26725.05), 64bit RyuJIT

Job=ShortRun  Toolchain=InProcessToolchain  IterationCount=5  
LaunchCount=1  WarmupCount=3  

```
|  Method | Count |     Mean |    Error |   StdDev | Scaled | ScaledSD |  Gen 0 | Allocated |
|-------- |------ |---------:|---------:|---------:|-------:|---------:|-------:|----------:|
| Updated |    10 | 107.1 ns | 5.906 ns | 1.534 ns |   1.00 |     0.00 | 0.0226 |     120 B |
| Classic |    10 | 141.9 ns | 5.659 ns | 1.470 ns |   1.33 |     0.02 | 0.0410 |     216 B |
