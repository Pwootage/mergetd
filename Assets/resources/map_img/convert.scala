import java.io.{FileWriter, PrintWriter, File}
import java.nio.file.{Files, Path, Paths}
import java.util.stream.Collectors
import javax.imageio.ImageIO

import scala.collection.JavaConversions._
import scala.collection.JavaConverters._

val files = Files.list(Paths.get("./")).collect(Collectors.toList[Path])

for (file: Path <- files if file.toString.endsWith(".png")) {
  val img = ImageIO.read(file.toFile)
  val out = file.toString.replace(".png", ".map.txt")
  println(file + "->" + out)
  val outStream = new PrintWriter(new FileWriter(new File(out)))

  for (y <- 0 until img.getHeight) {
    for (x <- 0 until img.getWidth) {
      val rgba = img.getRGB(x, y)
      val rgb = rgba & 0xFFFFFF
      val a = (rgba >> 24) & 0xFF
      if (a == 0) {
        outStream.print(" ")
      } else if (rgb == 0xFFFFFF) {
        outStream.print(".")
      } else {
        outStream.print("#")
      }
    }
    outStream.println()
  }
  outStream.close()
}
