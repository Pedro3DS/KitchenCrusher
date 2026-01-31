/// <summary>
/// Enum que define todos os tipos de glitch disponíveis no jogo.
/// Adicione novos tipos aqui conforme necessário.
/// </summary>
public enum GlitchType
{
    None = 0,
    CameraSwitch,       // Troca de câmera
    CameraShake,        // Tremor de câmera
    ScreenDistortion,   // Distorção visual (PSX effects)
    AudioGlitch,        // Glitch de áudio
    LightFlicker,       // Piscar de luzes
    EntitySpawn,        // Spawn de entidades (Ghost, Chaser, etc.)
    MovementReverse,    // Inversão de controles
    TimeWarp            // Alteração de velocidade do tempo
}
