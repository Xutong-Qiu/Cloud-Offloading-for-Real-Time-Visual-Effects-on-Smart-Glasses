package com.example.mydemo

import android.animation.ValueAnimator
import android.app.Activity
import android.opengl.Matrix
import android.content.Context
import android.opengl.EGL14
import android.opengl.EGLConfig
import android.opengl.EGLContext
import android.os.Build
import android.os.Bundle
import android.view.SurfaceView
import android.view.Choreographer
import android.view.MotionEvent
import android.view.Surface
import android.view.animation.LinearInterpolator
import androidx.annotation.RequiresApi
import com.google.android.filament.*
import com.google.android.filament.android.DisplayHelper
//import com.google.android.filament.android.FilamentHelper
import com.google.android.filament.android.UiHelper
import com.google.android.filament.ibl.Ibl
import com.google.android.filament.ibl.loadIbl
import com.google.android.filament.utils.TextureType
import com.google.android.filament.utils.loadTexture
import kotlin.math.PI
import kotlin.math.cos
import kotlin.math.sin
import java.nio.ByteBuffer
import java.nio.ByteOrder
import java.nio.channels.Channels

class MainActivity : Activity() {
    companion object {
        init {
            Filament.init()
        }
    }
    val context: Context
        get() = this

    private lateinit var surfaceView: SurfaceView
    // UiHelper is provided by Filament to manage SurfaceView and SurfaceTexture
    private lateinit var uiHelper: UiHelper
    // DisplayHelper is provided by Filament to manage the display
    private lateinit var displayHelper: DisplayHelper
    // Choreographer is used to schedule new frames
    private lateinit var choreographer: Choreographer
    private lateinit var engine: Engine
    // A renderer instance is tied to a single surface (SurfaceView, TextureView, etc.)
    private lateinit var renderer: Renderer
    // A scene holds all the renderable, lights, etc. to be drawn
    private lateinit var scene: Scene
    // A view defines a viewport, a scene and a camera for rendering
    private lateinit var view: View
    // Should be pretty obvious :)
    private lateinit var camera: Camera
    //private lateinit var mesh: Mesh
    private lateinit var ibl: Ibl
    private lateinit var material: Material
    private lateinit var materialInstance: MaterialInstance
    private lateinit var vertexBuffer: VertexBuffer
    private lateinit var indexBuffer: IndexBuffer
    private lateinit var streamHelper: StreamHelper
    //private lateinit var materialInstance: MaterialInstance
    @Entity private var renderable = 0
    @Entity private var light = 0
    // A swap chain is Filament's representation of a surface
    private var swapChain: SwapChain? = null

    private val frameScheduler = FrameCallback()

    private val animator = ValueAnimator.ofFloat(0.0f, 360.0f)


    private lateinit var baseColor: Texture
    private lateinit var normal: Texture
    private lateinit var aoRoughnessMetallic: Texture


    @RequiresApi(30)
    class Api30Impl {
        companion object {
            fun getDisplay(context: Context) = context.display!!
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        surfaceView = SurfaceView(this)
        setContentView(surfaceView)
        choreographer = Choreographer.getInstance()
        displayHelper = DisplayHelper(this)

        setupSurfaceView()
        setupFilament()
        setupView()
        setupScene()

        @Suppress("deprecation")
        val display = if (Build.VERSION.SDK_INT >= 30) {
            Api30Impl.getDisplay(this)
        } else {
            windowManager.defaultDisplay!!
        }

        streamHelper = StreamHelper(this, engine, materialInstance, display)
        this.title = streamHelper.getTestName()


    }

    private var touchX = 0f
    private var touchY = 0f
    private var cameraAngleX = 0.0
    private var cameraAngleY = 90.0
    private var radius = 1.0

    private fun updateCamera() {
        val cameraX = radius * cos(Math.toRadians(cameraAngleX)) * cos(Math.toRadians(cameraAngleY))
        val cameraY = radius * sin(Math.toRadians(cameraAngleX))
        val cameraZ = radius * cos(Math.toRadians(cameraAngleX)) * sin(Math.toRadians(cameraAngleY))
        camera.lookAt(cameraX, cameraY, cameraZ, 0.0, 0.0, 0.0, 0.0, -1.0, 0.0)
    }

    private fun setupSurfaceView() {
        uiHelper = UiHelper(UiHelper.ContextErrorPolicy.DONT_CHECK)
        uiHelper.renderCallback = SurfaceCallback()
        uiHelper.attachTo(surfaceView)

        surfaceView.setOnTouchListener { _, event ->
//            when(event.action){
//                MotionEvent.ACTION_DOWN -> {
//                    streamHelper.nextTest()
//                    this.title = streamHelper.getTestName()
//                }
//
//            }
//            super.onTouchEvent(event)
            when (event.action) {
                MotionEvent.ACTION_DOWN -> {
                    touchX = event.x
                    touchY = event.y

                }
                MotionEvent.ACTION_MOVE -> {
                    val deltaX = event.x - touchX
                    val deltaY = event.y - touchY
                    cameraAngleX -= deltaY * 0.5
                    cameraAngleY -= deltaX * 0.5
                    updateCamera()
                    touchX = event.x
                    touchY = event.y
                }
            }
            true
        }

    }

    private fun setupFilament() {
        val eglContext = createEGLContext()
        engine = Engine.create(eglContext)
        renderer = engine.createRenderer()
        scene = engine.createScene()
        view = engine.createView()
        camera = engine.createCamera(engine.entityManager.create())
    }

    private fun setupView() {
        val ssaoOptions = view.ambientOcclusionOptions
        ssaoOptions.enabled = true
        view.ambientOcclusionOptions = ssaoOptions

        if (engine.activeFeatureLevel == Engine.FeatureLevel.FEATURE_LEVEL_0) {
            // post-processing is not supported at feature level 0
            view.isPostProcessingEnabled = false
        } else {
            // NOTE: Try to disable post-processing (tone-mapping, etc.) to see the difference
            // view.isPostProcessingEnabled = false
        }

        // Tell the view which camera we want to use
        view.camera = camera

        // Tell the view which scene we want to render
        view.scene = scene
    }

    private fun setupScene() {
        loadMaterial()
        setupMaterial()
        createMesh()
        loadImageBasedLight()
        // To create a renderable we first create a generic entity
        scene.skybox = ibl.skybox
        scene.indirectLight = ibl.indirectLight


        // To create a renderable we first create a generic entity
        renderable = EntityManager.get().create()

        // We then create a renderable component on that entity
        // A renderable is made of several primitives; in this case we declare only 1
        RenderableManager.Builder(1)
            // Overall bounding box of the renderable
            .boundingBox(Box(0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.01f))
            // Sets the mesh data of the first primitive
            .geometry(0, RenderableManager.PrimitiveType.TRIANGLES, vertexBuffer, indexBuffer, 0, 6)
            // Sets the material of the first primitive
            .material(0, materialInstance)
            .build(engine, renderable)

//        engine.transformManager.setTransform(engine.transformManager.getInstance(renderable),
//            floatArrayOf(
//                1.0f,  0.0f, 0.0f, 0.0f,
//                0.0f,  1.0f, 0.0f, 0.0f,
//                0.0f,  0.0f, 1.0f, 0.0f,
//                0.0f,  0.0f, -10.0f, 1.0f
//            ))
        // Add the entity to the scene to render it
        scene.addEntity(renderable)

        light = EntityManager.get().create()
        val (r, g, b) = Colors.cct(6_500.0f)
        LightManager.Builder(LightManager.Type.DIRECTIONAL)
            .color(r, g, b)
            // Intensity of the sun in lux on a clear day
            .intensity(110_000.0f)
            // The direction is normalized on our behalf
            .direction(0.0f, -0.5f, -1.0f)
            .castShadows(true)
            .build(engine, light)

        // Add the entity to the scene to light it
        scene.addEntity(light)

        // Add the entity to the scene to render it
        camera.setExposure(16.0f, 1.0f / 125.0f, 100.0f)

        //this is the default value
        updateCamera()
        //camera.lookAt(0.0, 0.0, 2.0, 0.0, 0.0, 0.0, 0.0, -1.0, 0.0)
        //startAnimation()
    }

    private fun loadMaterial() {
        val name = "materials/lit.filamat"
        readUncompressedAsset(name).let {
            material = Material.Builder().payload(it, it.remaining()).build(engine)
        }

    }

    private fun createMesh() {
        val floatSize = 4
        val shortSize = 2

        // A vertex is a position: 3 floats for XYZ
        val vertexSize = 3 * floatSize + 4 * floatSize

        // Define a vertex and a function to put a vertex in a ByteBuffer
        @Suppress("ArrayInDataClass")
        data class Vertex(val x: Float, val y: Float, val z: Float, val tangents: FloatArray)
        fun ByteBuffer.put(v: Vertex): ByteBuffer {
            putFloat(v.x)
            putFloat(v.y)
            putFloat(v.z)
            v.tangents.forEach { putFloat(it) }
            return this
        }

        // 1 square, 4 vertices
        val vertexCount = 4
        val tfPZ = FloatArray(4)
        MathUtils.packTangentFrame( 1.0f,  0.0f, 0.0f, 0.0f, 1.0f,  0.0f,  0.0f,  0.0f,  1.0f, tfPZ)
        val vertexData = ByteBuffer.allocate(vertexCount * vertexSize)
            .order(ByteOrder.nativeOrder())
            // Square vertices
            .put(Vertex(-0.4f, -0.45f,  0.0f, tfPZ))
            .put(Vertex( 0.4f, -0.45f,  0.0f, tfPZ))
            .put(Vertex( 0.4f,  0.45f,  0.0f, tfPZ))
            .put(Vertex(-0.4f,  0.45f,  0.0f, tfPZ))
            .flip()

        // Declare the layout of our mesh
        vertexBuffer = VertexBuffer.Builder()
            .bufferCount(1)
            .vertexCount(vertexCount)
            .attribute(VertexBuffer.VertexAttribute.POSITION, 0, VertexBuffer.AttributeType.FLOAT3, 0, vertexSize)
            .build(engine)

        // Feed the vertex data to the mesh
        vertexBuffer.setBufferAt(engine, 0, vertexData)

        // Create the indices
        val indexData = ByteBuffer.allocate(2 * 3 * shortSize)
            .order(ByteOrder.nativeOrder())
            .putShort(0).putShort(1).putShort(2) // First triangle
            .putShort(0).putShort(2).putShort(3) // Second triangle
            .flip()

        // 1 square, 2 triangles
        val indexCount = vertexCount * 2
        indexBuffer = IndexBuffer.Builder()
            .indexCount(indexCount)
            .bufferType(IndexBuffer.Builder.IndexType.USHORT)
            .build(engine)

        indexBuffer.setBuffer(engine, indexData)
    }


    private fun setupMaterial() {
        // Create an instance of the material to set different parameters on it
        materialInstance = material.createInstance()
        materialInstance.setParameter("baseColor", Colors.RgbaType.SRGB, 5.0f, 0.85f, 0.57f, 0.0f)
        materialInstance.setParameter("roughness", 1.0f)

    }

    private fun loadImageBasedLight() {
        ibl = loadIbl(assets, "flower_road_no_sun_2k", engine)
        ibl.indirectLight.intensity = 40_000.0f
    }

    private fun startAnimation() {
        // Animate the triangle
        animator.interpolator = LinearInterpolator()
        animator.duration = 99_0000
        animator.repeatMode = ValueAnimator.RESTART
        animator.repeatCount = ValueAnimator.INFINITE
        animator.addUpdateListener { a ->
            val v = (a.animatedValue as Float)
            camera.lookAt(cos(v) * 3.0, 0.0, Math.abs(sin(v)) * 2.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0)
        }
        animator.start()
    }

    override fun onResume() {
        super.onResume()
        choreographer.postFrameCallback(frameScheduler)
        animator.start()
    }

    override fun onPause() {
        super.onPause()
        choreographer.removeFrameCallback(frameScheduler)
        animator.cancel()
    }

    override fun onDestroy() {
        super.onDestroy()

        // Stop the animation and any pending frame
        choreographer.removeFrameCallback(frameScheduler)
        animator.cancel()

        // Always detach the surface before destroying the engine
        uiHelper.detach()

        // Cleanup all resources
        engine.destroyEntity(renderable)
        engine.destroyRenderer(renderer)
        engine.destroyVertexBuffer(vertexBuffer)
        engine.destroyIndexBuffer(indexBuffer)
        engine.destroyMaterial(material)
        engine.destroyView(view)
        engine.destroyScene(scene)
        engine.destroyCameraComponent(camera.entity)

        val entityManager = EntityManager.get()
        entityManager.destroy(renderable)
        entityManager.destroy(camera.entity)

        // Destroying the engine will free up any resource you may have forgotten
        // to destroy, but it's recommended to do the cleanup properly
        engine.destroy()
    }

    inner class FrameCallback : Choreographer.FrameCallback {
        override fun doFrame(frameTimeNanos: Long) {
            // Schedule the next frame
            choreographer.postFrameCallback(this)

            // This check guarantees that we have a swap chain
            if (uiHelper.isReadyToRender) {
                // If beginFrame() returns false you should skip the frame
                // This means you are sending frames too quickly to the GPU
                if (renderer.beginFrame(swapChain!!, frameTimeNanos)) {
                    materialInstance.setParameter("uvOffset", streamHelper.uvOffset)
                    renderer.render(view)
                    renderer.endFrame()
                }
            }
        }
    }

    inner class SurfaceCallback : UiHelper.RendererCallback {
        override fun onNativeWindowChanged(surface: Surface) {
            swapChain?.let { engine.destroySwapChain(it) }

            // at feature level 0, we don't have post-processing, so we need to set
            // the colorspace to sRGB (FIXME: it's not supported everywhere!)
            var flags = uiHelper.swapChainFlags
            if (engine.activeFeatureLevel == Engine.FeatureLevel.FEATURE_LEVEL_0) {
                if (SwapChain.isSRGBSwapChainSupported(engine)) {
                    flags = flags or SwapChain.CONFIG_SRGB_COLORSPACE
                }
            }

            swapChain = engine.createSwapChain(surface, flags)
            displayHelper.attach(renderer, surfaceView.display)
        }

        override fun onDetachedFromSurface() {
            displayHelper.detach()
            swapChain?.let {
                engine.destroySwapChain(it)
                engine.flushAndWait()
                swapChain = null
            }
        }

        override fun onResized(width: Int, height: Int) {
            val aspect = width.toDouble() / height.toDouble()
            camera.setProjection(45.0, aspect, 0.1, 20.0, Camera.Fov.VERTICAL)

            view.viewport = Viewport(0, 0, width, height)

        }
    }

    private fun readUncompressedAsset(assetName: String): ByteBuffer {
        val fd = assets.openFd(assetName)
        fd.use {
            val input = fd.createInputStream()
            val dst = ByteBuffer.allocate(fd.length.toInt())
            val src = Channels.newChannel(input)
            src.read(dst)
            src.close()
            val rewoundBuffer = dst.apply { rewind() }
            return rewoundBuffer
        }
    }
    private fun createEGLContext(): EGLContext {
        // Providing this constant here (rather than using EGL_OPENGL_ES3_BIT ) allows us to use a lower target API for this project.
        val kEGLOpenGLES3Bit = 64

        val shareContext = EGL14.EGL_NO_CONTEXT
        val display = EGL14.eglGetDisplay(EGL14.EGL_DEFAULT_DISPLAY)

        val minorMajor: IntArray = IntArray(2)
        EGL14.eglInitialize(display, minorMajor, 0, minorMajor, 1)
        val configs = arrayOfNulls<EGLConfig>(1)
        val numConfig = intArrayOf(0)
        val attribs = intArrayOf(EGL14.EGL_RENDERABLE_TYPE, kEGLOpenGLES3Bit, EGL14.EGL_NONE)
        EGL14.eglChooseConfig(display, attribs, 0, configs, 0, 1, numConfig, 0)

        val contextAttribs = intArrayOf(EGL14.EGL_CONTEXT_CLIENT_VERSION, 3, EGL14.EGL_NONE)
        val context = EGL14.eglCreateContext(display, configs[0], shareContext, contextAttribs, 0)

        val surfaceAttribs = intArrayOf(EGL14.EGL_WIDTH, 1, EGL14.EGL_HEIGHT, 1, EGL14.EGL_NONE)

        val surface = EGL14.eglCreatePbufferSurface(display, configs[0], surfaceAttribs, 0)

        check(EGL14.eglMakeCurrent(display, surface, surface, context)) { "Error making GL context." }
        return context
    }

}

