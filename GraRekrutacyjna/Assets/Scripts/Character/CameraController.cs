using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Camera; // kamera
    private Vector3 CameraStartingPostion; // zapisanie pozycji startowej kamery

    private float mouseMoveTreshold = 0.2f; // margines b³êdu drgañ oddzia³uj¹cych na mysz, bez tego minimalne ruchy myszk¹ s¹ wykrywane jako aktywnoœæ gracza
    public float IdleCameraMoveDistance = -3f; // ustalenien o ile jednostek ma siê przesun¹æ kamera
    public float IdleTime = 3f; // czas po jakim kamera ma zacz¹æ siê oddalaæ
    public float cameraMoveDuration = 2f; // czas w jakim kamera ma sie oddaliæ
    private Vector3 lastMousePosition; // zapisanie ostatniej pozycji myszki

    private bool stopLerping = false; // je¿eli stopLerping jest true przerywa cofanie kamery je¿eli po 3 sekundach aktywnoœci gracz wykona akcje przed pe³nym oddaleniem kamery
    private float Timer = 0f; // zmienna licznika czasu braku aktywnoœci
    public float Sensitivity; // zmienna czu³oœci myszki
    float xRotation; // zmienna przesuniêcia wzglêdem osi X

    void Start()
    {
        Cursor.visible = false; // wy³¹czenie widocznoœci kursora
        CameraStartingPostion = Camera.localPosition; // zapisanie startowej pozycji kamery
    }

    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * Sensitivity; // pobranie przesuniêcia myszki w osi x
        float MouseY = Input.GetAxis("Mouse Y") * Sensitivity; // pobranie przesuniecia myszki w osi y

        IdleTimer(); // funkcja przesuwania kamery
        transform.Rotate(Vector3.up * MouseX); // przesuniêcie kamery wzglêdem osi y
        xRotation -= MouseY; 
        xRotation = Mathf.Clamp(xRotation, 0, 35); // ograniczenie przsuwania w osi x 
        Camera.localRotation = Quaternion.Euler(xRotation, 0, 0); // przesuniêcie osi x o watoœæ xRotation

    }

    void IdleTimer() // licznik czasu bezczynnoœci
    {
        if (Input.anyKey || IsMouseMoving()) // sprawdzamy czy gracz wróci³ do gry
        {
            Timer = 0f; // zresetowanie licznika bezczynnoœci
            if(Camera.localPosition.z != CameraStartingPostion.z) // sprawdamy czy kamera jest oddalona od gracza
            {
                stopLerping = true; // zatrzymujemy oddalanie kamery je¿eli gracz wróci³ zanim kamera w pe³ni siê oddali³a
                StartCoroutine(LerpValueRetrunToBasePosition(Camera.localPosition.z, CameraStartingPostion.z)); // rozpoczynamy coroutine zbli¿ania kamery na bazow¹ odleg³oœæ
            }
        }
        else // je¿eli gracz nie wykonuje akcji
        {
            Timer += Time.deltaTime; // zwiekszamy licznik co sekunde;

            if(Timer >= IdleTime) // je¿eli licznik jest wiekszy ni¿ ustalony czas idle(3s)
            {
                StartCoroutine(LerpValueIdle(Camera.localPosition.z, CameraStartingPostion.z + IdleCameraMoveDistance)); // rozpoczynamy coroutine oddalania kamery

            }
        }
    }

    bool IsMouseMoving() // sprawdzamy czy gracz porusza myszk¹
    {
        Vector3 currentMousePosition = Input.mousePosition; // pobieramy pozycje myszy
        Vector3 mousePositionDifference = currentMousePosition - lastMousePosition; // przesuniêcie myszy

        bool mouseMoving = mousePositionDifference.sqrMagnitude > mouseMoveTreshold * mouseMoveTreshold; 
        // obliczamy kwadrat magnitudy wektora z ró¿nicy pozycji myszy w celu obliczenia przesuniêcia kursora myszy, sprawdzamy czy jest wiekszy od kwadratu marginesu b³êdu
        lastMousePosition = currentMousePosition; // ustawiamy ostatni¹ pozycje myszy na obecn¹ do póŸniejszego sprawdzenia ró¿nicy
        return mouseMoving; // zwracamy true je¿eli gracz poruszy³ myszk¹, false je¿eli nie
    }

    IEnumerator LerpValueIdle(float startZ, float endZ) // liniowa interpolacja oddalenia kamery w przypadku braku aktywnoœci
    {
        float timeElapsed = 0; // zmienna pomocnicza zapisuj¹ca ile czasu mine³o
        
        while (timeElapsed < cameraMoveDuration) // je¿eli czas jest mniejszy ni¿ ustalony czas po jakim ma wykonaæ siê przesuniêcie przesuwamy kamere
        {
            if (stopLerping) // je¿eli gracz wykona³ akcje cofanie kamery zostanie przerwane
            {
                yield break; // koñczymy coroutine
            }

            float t = timeElapsed / cameraMoveDuration; // wartoœæ z zakresu 0 - 1 okreœlaj¹ca gdze w zakresie a b znajduje siê wartoœæ
                float lerpedValue = Mathf.Lerp(startZ, endZ, t); // wykonanie obliczeñ interpolacji
                timeElapsed += Time.deltaTime; // dodanie czasu do licznika

                Camera.localPosition = new Vector3(Camera.localPosition.x, Camera.localPosition.y, lerpedValue); // ustawienie poœredniej pozycji kamery

                yield return null;
            
        }
        Debug.Log("koncze oddalenie");
    }
    IEnumerator LerpValueRetrunToBasePosition(float startZ, float endZ) // liniowa interpolacja zbli¿ania kamery 
    {
        float timeElapsed = 0; // zmienna pomocnicza zapisuj¹ca ile czasu mine³o

        while(timeElapsed < cameraMoveDuration) // je¿eli czas jest mniejszy ni¿ ustalony czas po jakim ma wykonaæ siê przesuniêcie przesuwamy kamere
        {
            if (Camera.localPosition == CameraStartingPostion) // je¿eli pozycja kamery jest ju¿ w odpowiednim miejscu przerywamy coroutine
            {
                yield break;
            }
            float t = timeElapsed / cameraMoveDuration; 
            float lerpedValue = Mathf.Lerp(startZ, endZ, t);
            timeElapsed += Time.deltaTime;

            Camera.localPosition = new Vector3(Camera.localPosition.x, Camera.localPosition.y, lerpedValue);

            yield return null;
        }

        Camera.localPosition = new Vector3(Camera.localPosition.x, Camera.localPosition.y, endZ);
        stopLerping = false;
    }


}
