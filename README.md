# esp8266-oled-videostream

#### this a simple proof of concept to test the videostreaming possibility of an esp8266 and low res oled screen.


## how it work's

i've create a simple c# application which captures screen, resizes and dithering it to get a suitable image for an 128x64 monochrome oled screen (ssd1306 in my case).

the image is translated into zeros and ones to represent black and white pixels and choped into packages.

that packages are sent via udp to an esp8266 with attached oled screen who reads the packages and sets the pixels to desired color, as a hole frame is recieved the display updates.

## know issues

the capturing is very resource hungry cause of it's not for contiues capturing but for my testing purposes it was enough, you can get higher framerates by lower screen resolution (try lowest).

>i think it's possible to get even higher framerates by faster sending of packages (i reached around 20-25fps max from my capturing application on 800x600)
