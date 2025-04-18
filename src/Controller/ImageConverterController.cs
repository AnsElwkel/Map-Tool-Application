using System;
//using Map_Creation_Tool.src.Model;

namespace Map_Creation_Tool.src.Controller
{
    public class ImageConverterController
    {
        Image img;
        public ImageConverterController() { }

        //Before Load the image, we need to show the validation message
        public void takeImage(Image img)
        {
            //Validate the image
            this.img = img;
        }

        public void convertImage()
        {
            //Call the image converter
            Model.ImageConverter imageConverter = new Model.ImageConverter();
            imageConverter.convert(img);
        }
    }
}
