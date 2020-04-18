<?php

$img = imagecreatetruecolor(1024, 1024);

for ($i=0;$i<2000;++$i){
    imagesetpixel($img, mt_rand(0, 1023), mt_rand(0, 1023), imagecolorallocate($img, 255, 255, 255));
}

imagepng($img, 'starfield.png');
