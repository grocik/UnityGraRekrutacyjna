using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Camera; // kamera
    private Vector3 CameraStartingPostion; // zapisanie pozycji startowej kamery

    private float mouseMoveTreshold = 0.2f; // margines b��du drga� oddzia�uj�cych na mysz, bez tego minimalne ruchy myszk� s� wykrywane jako aktywno�� gracza
    public float IdleCameraMoveDistance = -3f; // ustalenien o ile jednostek ma si� przesun�� kamera
    public float IdleTime = 3f; // czas po jakim kamera ma zacz�� si� oddala�
    public float cameraMoveDuration = 2f; // czas w jakim kamera ma sie oddali�
    private Vector3 lastMousePosition; // zapisanie ostatniej pozycji myszki

    private bool stopLerping = false; // je�eli stopLerping jest true przerywa cofanie kamery je�eli po 3 sekundach aktywno�ci gracz wykona akcje przed pe�nym oddaleniem kamery
    private float Timer = 0f; // zmienna licznika czasu braku aktywno�ci
    public float Sensitivity; // zmienna czu�o�ci myszki
    float xRotation; // zmienna przesuni�cia wzgl�dem osi X

    void Start()
    {
        Cursor.visible = false; // wy��czenie widoczno�ci kursora
        CameraStartingPostion = Camera.localPosition; // zapisanie startowej pozycji kamery
    }

    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * Sensitivity; // pobranie przesuni�cia myszki w osi x
        float MouseY = Input.GetAxis("Mouse Y") * Sensitivity; // pobranie przesuniecia myszki w osi y

        IdleTimer(); // funkcja przesuwania kamery
        transform.Rotate(Vector3.up * MouseX); // przesuni�cie kamery wzgl�dem osi y
        xRotation -= MouseY; 
        xRotation = Mathf.Clamp(xRotation, 0, 35); // ograniczenie przsuwania w osi x 
        Camera.localRotation = Quaternion.Euler(xRotation, 0, 0); // przesuni�cie osi x o wato�� xRotation

    }

    void IdleTimer() // licznik czasu bezczynno�ci
    {
        if (Input.anyKey || IsMouseMoving()) // sprawdzamy czy gracz wr�ci� do gry
        {
            Timer = 0f; // zresetowanie licznika bezczynno�ci
            if(Camera.localPosition.z != CameraStartingPostion.z) // sprawdamy czy kamera jest oddalona od gracza
            {
                stopLerping = true; // zatrzymujemy oddalanie kamery je�eli gracz wr�ci� zanim kamera w pe�ni si� oddali�a
                StartCoroutine(LerpValueRetrunToBasePosition(Camera.localPosition.z, CameraStartingPostion.z)); // rozpoczynamy coroutine zbli�ania kamery na bazow� odleg�o��
            }
        }
        else // je�eli gracz nie wykonuje akcji
        {
            Timer += Time.deltaTime; // zwiekszamy licznik co sekunde;

            if(Timer >= IdleTime) // je�eli licznik jest wiekszy ni� ustalony czas idle(3s)
            {
                StartCoroutine(LerpValueIdle(Camera.localPosition.z, CameraStartingPostion.z + IdleCameraMoveDistance)); // rozpoczynamy coroutine oddalania kamery

            }
        }
    }

    bool IsMouseMoving() // sprawdzamy czy gracz porusza myszk�
    {
        Vector3 currentMousePosition = Input.mousePosition; // pobieramy pozycje myszy
        Vector3 mousePositionDifference = currentMousePosition - lastMousePosition; // przesuni�cie myszy

        bool mouseMoving = mousePositionDifference.sqrMagnitude > mouseMoveTreshold * mouseMoveTreshold; 
        // obliczamy kwadrat magnitudy wektora z r�nicy pozycji myszy w celu obliczenia przesuni�cia kursora myszy, sprawdzamy czy jest wiekszy od kwadratu marginesu b��du
        lastMousePosition = currentMousePosition; // ustawiamy ostatni� pozycje myszy na obecn� do p�niejszego sprawdzenia r�nicy
        return mouseMoving; // zwracamy true je�eli gracz poruszy� myszk�, false je�eli nie
    }

    IEnumerator LerpValueIdle(float startZ, float endZ) // liniowa interpolacja oddalenia kamery w przypadku braku aktywno�ci
    {
        float timeElapsed = 0; // zmienna pomocnicza zapisuj�ca ile czasu mine�o
        
        while (timeElapsed < cameraMoveDuration) // je�eli czas jest mniejszy ni� ustalony czas po jakim ma wykona� si� przesuni�cie przesuwamy kamere
        {
            if (stopLerping) // je�eli gracz wykona� akcje cofanie kamery zostanie przerwane
            {
                yield break; // ko�czymy coroutine
            }

            float t = timeElapsed / cameraMoveDuration; // warto�� z zakresu 0 - 1 okre�laj�ca gdze w zakresie a b znajduje si� warto��
                float lerpedValue = Mathf.Lerp(startZ, endZ, t); // wykonanie oblicze� interpolacji
                timeElapsed += Time.deltaTime; // dodanie czasu do licznika

                Camera.localPosition = new Vector3(Camera.localPosition.x, Camera.localPosition.y, lerpedValue); // ustawienie po�redniej pozycji kamery

                yield return null;
            
        }
        Debug.Log("koncze oddalenie");
    }
    IEnumerator LerpValueRetrunToBasePosition(float startZ, float endZ) // liniowa interpolacja zbli�ania kamery 
    {
        float timeElapsed = 0; // zmienna pomocnicza zapisuj�ca ile czasu mine�o

        while(timeElapsed < cameraMoveDuration) // je�eli czas jest mniejszy ni� ustalony czas po jakim ma wykona� si� przesuni�cie przesuwamy kamere
        {
            if (Camera.localPosition == CameraStartingPostion) // je�eli pozycja kamery jest ju� w odpowiednim miejscu przerywamy coroutine
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
