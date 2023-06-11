package com.example.mydemo
import android.content.Context
import android.graphics.*
import android.media.ImageReader
import android.media.MediaPlayer
import android.opengl.GLES11Ext
import android.opengl.GLES30
import android.opengl.Matrix
import android.os.Handler
import android.os.Looper
import android.util.Size
import android.view.Display
import android.view.Surface
import com.google.android.filament.*


/**
 * Demonstrates Filament's various texture sharing mechanisms.
 */
class StreamHelper(
    private val activity: MainActivity,
    private val filamentEngine: Engine,
    private val filamentMaterial: MaterialInstance,
    private val display: Display,
) {
    /**
     * The StreamSource configures the source data for the texture.
     *
     * All tests draw animated test stripes as follows:
     *
     *  - The left stripe uses texture-based animation via Android's 2D drawing API.
     *  - The right stripe uses shader-based animation.
     *
     * Ideally these are perfectly in sync with each other.
     */
    enum class StreamSource {
        CANVAS_STREAM_NATIVE,     // copy-free but does not guarantee synchronization
        CANVAS_STREAM_ACQUIRED,   // synchronized and copy-free
    }

    private lateinit var mediaPlayer: MediaPlayer
    private var textureId = 0

    private var streamSource = StreamSource.CANVAS_STREAM_NATIVE
    private val directImageHandler = Handler(Looper.getMainLooper())
    private var resolution = Size(640, 480)
    private var surfaceTexture: SurfaceTexture? = null
    private var imageReader: ImageReader? = null
    private var frameNumber = 0L
    private var canvasSurface: Surface? = null
    private var filamentTexture: Texture? = null
    private var filamentStream: Stream? = null
    var uvOffset = 0.0f
        private set

    private val kGradientSpeed = 5
    private val kGradientCount = 5

    private val kGradientColors = intArrayOf(
        Color.RED, Color.RED,
        Color.WHITE, Color.WHITE,
        Color.GREEN, Color.GREEN,
        Color.WHITE, Color.WHITE,
        Color.BLUE, Color.BLUE)

    private val kGradientStops = floatArrayOf(
        0.0f, 0.1f,
        0.1f, 0.5f,
        0.5f, 0.6f,
        0.6f, 0.9f,
        0.9f, 1.0f)

    // This seems a little high, but lower values cause occasional "client tried to acquire more than maxImages buffers" on a Pixel 3.
    private val kImageReaderMaxImages = 7

    init {
        startTest()
    }

    fun repaintCanvas() {
        val kGradientScale = resolution.width.toFloat() / kGradientCount
        val kGradientOffset = (frameNumber.toFloat() * kGradientSpeed) % resolution.width
        val surface = canvasSurface
        if (surface != null) {
            val canvas = surface.lockCanvas(null)
            val bitmap = Bitmap.createBitmap(resolution.width, resolution.height, Bitmap.Config.ARGB_8888)
            val offscreenCanvas = Canvas(bitmap)

            //... Draw onto offscreenCanvas instead of canvas ...
            // Replace all instances of 'canvas' with 'offscreenCanvas' in this function

            canvas.drawBitmap(bitmap, 0f, 0f, null)

            surface.unlockCanvasAndPost(canvas)


            val movingPaint = Paint()
            movingPaint.shader = LinearGradient(
                kGradientOffset,
                0.0f,
                kGradientOffset + kGradientScale,
                0.0f,
                kGradientColors,
                kGradientStops,
                Shader.TileMode.REPEAT
            )
            offscreenCanvas.drawRect(
                Rect(0, resolution.height / 2, resolution.width, resolution.height),
                movingPaint
            )

            val staticPaint = Paint()
            staticPaint.shader = LinearGradient(
                0.0f,
                0.0f,
                kGradientScale,
                0.0f,
                kGradientColors,
                kGradientStops,
                Shader.TileMode.REPEAT
            )
            offscreenCanvas.drawRect(Rect(0, 0, resolution.width, resolution.height / 2), staticPaint)

            surface.unlockCanvasAndPost(offscreenCanvas)

            if (streamSource == StreamSource.CANVAS_STREAM_ACQUIRED) {
                val image = imageReader!!.acquireLatestImage()
                filamentStream!!.setAcquiredImage(
                    image.hardwareBuffer!!,
                    directImageHandler
                ) { image.close() }
            }
        }

        frameNumber++
        uvOffset = 1.0f - kGradientOffset / resolution.width.toFloat()
    }

    fun nextTest() {
        stopTest()
        streamSource = StreamSource.values()[(streamSource.ordinal + 1) % StreamSource.values().size]
        startTest()
    }

    fun getTestName(): String {
        return streamSource.name
    }

    private fun startTest() {

        // Create and set up a new MediaPlayer
        mediaPlayer = MediaPlayer.create(activity.context, R.raw.test2) // Replace with your own video file

        // Create the Filament Texture and Sampler objects.
        filamentTexture = Texture.Builder()
            .sampler(Texture.Sampler.SAMPLER_EXTERNAL)
            .format(Texture.InternalFormat.RGBA8)
            .build(filamentEngine)

        val filamentTexture = this.filamentTexture!!

        val sampler = TextureSampler(
            TextureSampler.MinFilter.LINEAR, TextureSampler.MagFilter.LINEAR,
            TextureSampler.WrapMode.REPEAT)

        // We are texturing a front-facing square shape so we need to generate a matrix that transforms (u, v, 0, 1)
        // into a new UV coordinate according to the screen rotation and the aspect ratio of the camera image.
        val aspectRatio = resolution.width.toFloat() / resolution.height.toFloat()
        val textureTransform = FloatArray(16)
        Matrix.setIdentityM(textureTransform, 0)
        when (display.rotation) {
//            Surface.ROTATION_0 -> {
//                Matrix.translateM(textureTransform, 0, 0.0f, 0.5f, 0.0f)
//                Matrix.rotateM(textureTransform, 0, 180.0f, 0.0f, 0.0f, 1.0f)
//                Matrix.translateM(textureTransform, 0, -0.5f, -0.3f, 0.0f)
//                Matrix.scaleM(textureTransform, 0, 0.7f, 0.7f , 0.7f)
//            }
//            Surface.ROTATION_90 -> {
//                Matrix.translateM(textureTransform, 0, 1.0f, 1.0f, 0.0f)
//                Matrix.rotateM(textureTransform, 0, 180.0f, 0.0f, 0.0f, 1.0f)
//                Matrix.translateM(textureTransform, 0, 1.0f, 0.0f, 0.0f)
//                Matrix.scaleM(textureTransform, 0, -1.0f / aspectRatio, 1.0f, 1.0f)
//            }
//            Surface.ROTATION_270 -> {
//                Matrix.translateM(textureTransform, 0, 1.0f, 0.0f, 0.0f)
//                Matrix.scaleM(textureTransform, 0, -1.0f / aspectRatio, 1.0f, 1.0f)
//            }
        }

        // Connect the Stream to the Texture and the Texture to the MaterialInstance.
        filamentMaterial.setParameter("videoTexture", filamentTexture, sampler)
        filamentMaterial.setParameter("textureTransform",
            MaterialInstance.FloatElement.MAT4, textureTransform, 0, 1)

        if (streamSource == StreamSource.CANVAS_STREAM_NATIVE) {

            // Create the Android surface that will hold the canvas image.
            textureId = createExternalTextureId()
            val surfaceTexture = SurfaceTexture(textureId)
            mediaPlayer.setSurface(Surface(surfaceTexture))
            mediaPlayer.setOnPreparedListener { it.start() }

//            surfaceTexture = SurfaceTexture(0)
            surfaceTexture!!.setDefaultBufferSize(resolution.width, resolution.height)
            surfaceTexture!!.detachFromGLContext()
//            canvasSurface = Surface(surfaceTexture)

            // Create the Filament Stream object that gets bound to the Texture.
            filamentStream = Stream.Builder()
                .stream(surfaceTexture!!)
                .build(filamentEngine)

            filamentTexture.setExternalStream(filamentEngine, filamentStream!!)
        }

        if (streamSource == StreamSource.CANVAS_STREAM_ACQUIRED) {
            filamentStream = Stream.Builder()
                .width(resolution.width)
                .height(resolution.height)
                .build(filamentEngine)

            filamentTexture.setExternalStream(filamentEngine, filamentStream!!)

            this.imageReader = ImageReader.newInstance(
                resolution.width, resolution.height, ImageFormat.FLEX_RGBA_8888, kImageReaderMaxImages
            ).apply {
                canvasSurface = surface
            }
        }

        // Draw the first frame now.
        frameNumber = 0
        //repaintCanvas()
    }

    private fun createExternalTextureId(): Int {
        // Step 1: Create external texture
        val textures = IntArray(1)
        GLES30.glGenTextures(1, textures, 0)
        textureId = textures[0]

        GLES30.glBindTexture(GLES11Ext.GL_TEXTURE_EXTERNAL_OES, textureId)
        return textureId
    }

    private fun stopTest() {
        filamentTexture?.let { filamentEngine.destroyTexture(it) }
        filamentStream?.let { filamentEngine.destroyStream(it) }
        surfaceTexture?.release()
    }
}