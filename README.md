# TVSharp

A decompilation of TVSharp via dnSpy.

## I AM NOT THE ORIGINAL AUTHOR OF THIS SOFTWARE

The original software comes from [rtl-sdr.ru](https://web.archive.org/web/20210330131151/http://rtl-sdr.ru/uploads/download/tvsharp.zip) and I am not permitted to redistribute it. I am only hosting this repository for educational purposes.

## What is TVSharp?

TVSharp is a software defined radio (SDR) application that allows you to receive and decode analog NTSC/PAL television signals. It is written in C# and uses the [RTL-SDR](https://www.rtl-sdr.com/) as the receiver.

## Why decompile it?

I'm interested in the software defined radio community and I wanted to learn more about how SDR applications work. I'm particularly fond of the amateur television part of the hobby and I wanted to make it more accessible for others to receive a signal from their [local US repeater](https://www.atn-tv.com/repeaters/) (I am not affiliated with ATN-TV).

## Limitations

- Only works with the RTL-SDR
- Only works on Windows
- No support for color, only black and white
- No support for audio (FM demodulation can be done with a spare SDR or other receiver)

## Differences

- x64 compilation
- Open source

## How to use

1. Install [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)
    - Make sure to download the `.NET Desktop Runtime` for x64.
2. Download the latest `TVSharp.zip` release from the [releases page](https://github.com/TeamPopplio/TVSharp/releases)
    - Do not download the source code zip file
3. Extract the zip file to a folder
4. Plug in a compatible RTL-SDR device
5. Run `TVSharp.exe`
6. Set a frequency
7. Click the `Start` button

## Building

```bash
git clone https://github.com/TeamPopplio/TVSharp.git
cd TVSharp
dotnet build
```

Requires an x64 version of `rtlsdr.dll` and related dll files.

## License

This project has an unknown license.

## Contact

If you have any questions, feel free to contact me on [Twitter](https://twitter.com/TeamPopplio).

Alternatively, use my project [Discord](https://discord.gg/3X3teNecWs).

I may not be able to guarantee support, but I will try my best.
