
#if defined(MATERIAL_HAS_REFLECTIONS)
highp float linearizeDepth(highp float depth) {
const highp float preventDiv0 = 1.0 / 16777216.0;
highp mat4 p = getViewFromClipMatrix();
return (depth * p[2].z + p[3].z) / max(depth * p[2].w + p[3].w, preventDiv0);
}
void swap(inout highp float a, inout highp float b) {
highp float temp = a;
a = b;
b = temp;
}
highp float distanceSquared(highp vec2 a, highp vec2 b) {
a -= b;
return dot(a, a);
}
bool traceScreenSpaceRay(const highp vec3 vsOrigin, const highp vec3 vsDirection,
const highp mat4 uvFromViewMatrix, const mediump sampler2D vsZBuffer,
const float vsZThickness, const highp float nearPlaneZ, const float stride,
const float jitterFraction, const highp float maxSteps, const float maxRayTraceDistance,
out highp vec2 hitPixel, out highp vec3 vsHitPoint) {
highp float rayLength = ((vsOrigin.z + vsDirection.z * maxRayTraceDistance) > nearPlaneZ) ?
(nearPlaneZ - vsOrigin.z) / vsDirection.z : maxRayTraceDistance;
highp vec3 vsEndPoint = vsDirection * rayLength + vsOrigin;
highp vec4 H0 = mulMat4x4Float3(uvFromViewMatrix, vsOrigin);
highp vec4 H1 = mulMat4x4Float3(uvFromViewMatrix, vsEndPoint);
highp float k0 = 1.0 / H0.w;
highp float k1 = 1.0 / H1.w;
highp vec3 Q0 = vsOrigin * k0;
highp vec3 Q1 = vsEndPoint * k1;
highp vec2 P0 = H0.xy * k0;
highp vec2 P1 = H1.xy * k1;
hitPixel = vec2(-1.0, -1.0);
P1 += vec2((distanceSquared(P0, P1) < 0.0001) ? 0.01 : 0.0);
highp vec2 delta = P1 - P0;
bool permute = false;
if (abs(delta.x) < abs(delta.y)) {
permute = true;
delta = delta.yx;
P1 = P1.yx;
P0 = P0.yx;
}
float stepDirection = sign(delta.x);
highp float invdx = stepDirection / delta.x;
highp vec2 dP = vec2(stepDirection, invdx * delta.y);
highp vec3  dQ = (Q1 - Q0) * invdx;
highp float dk = (k1 - k0) * invdx;
dP *= stride; dQ *= stride; dk *= stride;
P0 += dP * jitterFraction; Q0 += dQ * jitterFraction; k0 += dk * jitterFraction;
highp vec3  Q = Q0;
highp float k = k0;
highp float prevZMaxEstimate = vsOrigin.z;
highp float stepCount = 0.0;
highp float rayZMax = prevZMaxEstimate;
highp float rayZMin = prevZMaxEstimate;
highp float sceneZMax = rayZMax + 1e4;
highp float end = P1.x * stepDirection;
for (highp vec2 P = P0;
((P.x * stepDirection) <= end) &&
(stepCount < maxSteps) &&
((rayZMax < sceneZMax - vsZThickness) ||
(rayZMin > sceneZMax)) &&
(sceneZMax != 0.0);
P += dP, Q.z += dQ.z, k += dk, stepCount += 1.0) {
hitPixel = permute ? P.yx : P;
rayZMin = prevZMaxEstimate;
rayZMax = (dQ.z * 0.5 + Q.z) / (dk * 0.5 + k);
prevZMaxEstimate = rayZMax;
if (rayZMin > rayZMax) { swap(rayZMin, rayZMax); }
sceneZMax = linearizeDepth(texelFetch(vsZBuffer, int2(hitPixel), 0).r);
}
Q.xy += dQ.xy * stepCount;
vsHitPoint = Q * (1.0 / k);
return (rayZMax >= sceneZMax - vsZThickness) && (rayZMin <= sceneZMax);
}
highp mat4 scaleMatrix(const highp float x, const highp float y) {
mat4 m = mat4(1.0f);
m[0].x = x;
m[1].y = y;
m[2].z = 1.0f;
m[3].w = 1.0f;
return m;
}
vec4 evaluateScreenSpaceReflections(const highp vec3 wsRayDirection) {
vec4 Fr = vec4(0.0f);
highp vec3 wsRayStart = shading_position + frameUniforms.ssrBias * wsRayDirection;
highp vec3 vsOrigin = mulMat4x4Float3(getViewFromWorldMatrix(), wsRayStart).xyz;
highp vec3 vsDirection = mulMat3x3Float3(getViewFromWorldMatrix(), wsRayDirection);
float vsZThickness = frameUniforms.ssrThickness;
highp float nearPlaneZ = -frameUniforms.nearOverFarMinusNear / frameUniforms.oneOverFarMinusNear;
float stride = frameUniforms.ssrStride;
highp vec2 fragCoord = gl_FragCoord.xy;
fragCoord += vec2(frameUniforms.temporalNoise);
float jitterFraction = interleavedGradientNoise(fragCoord);
float maxRayTraceDistance = frameUniforms.ssrDistance;
highp vec2 res = vec2(textureSize(light_structure, 0).xy);
highp mat4 uvFromViewMatrix =
scaleMatrix(res.x, res.y) *
frameUniforms.ssrUvFromViewMatrix;
highp float maxSteps = float(max(res.x, res.y));
highp vec2 hitPixel;
highp vec3 vsHitPoint;
if (traceScreenSpaceRay(vsOrigin, vsDirection, uvFromViewMatrix, light_structure,
vsZThickness, nearPlaneZ, stride, jitterFraction, maxSteps,
maxRayTraceDistance, hitPixel, vsHitPoint)) {
highp vec4 reprojected = mulMat4x4Float3(frameUniforms.ssrReprojection, vsHitPoint);
reprojected.xy *= (1.0 / reprojected.w);
const float fadeRateEdge = 12.0f;
const float fadeRateDistance = 4.0f;
vec2 edgeFactor = max(fadeRateEdge * abs(reprojected.xy - 0.5f) - (fadeRateEdge * 0.5f - 1.0f), 0.0f);
float fade = saturate(1.0 - dot(edgeFactor, edgeFactor));
float t = distance(vsOrigin, vsHitPoint) / maxRayTraceDistance;
fade *= saturate(fadeRateDistance - fadeRateDistance * t);
fade *= (1.0 - max(0.0, vsDirection.z));
Fr = vec4(textureLod(light_ssr, reprojected.xy, 0.0).rgb * fade, fade);
}
return Fr;
}
#endif
