# SpineExporter

Command line program to use [SpineViewer](https://github.com/ww-rm/SpineViewer)

Based on [ww-rm/SpineViewer#43](https://github.com/ww-rm/SpineViewer/issues/43)

Recommended usage:
```sh
SpineViewer.exe --skel "character.skel" --animation "Idle" --output "preview.webm" --pma
```

```
usage: SpineExporter.exe [--skel PATH] [--atlas PATH] [--output PATH] [--animation STR] [--pma] [--fps INT] [--loop] [--crf INT] [--width INT] [--height INT] [--centerx INT] [--centery INT] [--zoom FLOAT] [--quiet]

options:
  --skel PATH           Path to the .skel file
  --atlas PATH          Path to the .atlas file, default searches in the skel file directory
  --output PATH         Output file path
  --animation STR       Animation name
  --pma                 Use premultiplied alpha, default false
  --fps INT             Frames per second, default 24
  --loop                Whether to loop the animation, default false
  --crf INT             Constant Rate Factor i.e. video quality, from 0 (lossless) to 51 (worst), default 23
  --width INT           Output width, default 512
  --height INT          Output height, default 512
  --centerx INT         Center X offset, default automatically finds bounds
  --centery INT         Center Y offset, default automatically finds bounds
  --zoom FLOAT          Zoom level, default 1.0
  --quiet               Removes console progress log, default false
```
