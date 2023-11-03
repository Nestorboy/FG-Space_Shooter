# FG-Space_Shooter

I decided to do this assignment using Unity's DOTS family of packages. DOTS is a framework for designing Unity projects mainly with the data-oriented and entity component system architectural patterns in mind.

### DOD & ECS
Oftentimes game development is riddled with bodged together solutions for the sake of efficiency, but as hardware and technology evolves, these 'efficient' implementations struggle to make proper use of the hardware that's actually available to us today.
The Data Oriented Design pattern aims to emphisize how computers internally process data by fetching and structuring memory in a linear fashion. The reason that we want have linear memory access is that it properly takes advantage of the several caches our CPUs come with today. By making proper use of the local caches (L1, L2 and shared L3), we're able to reduce how much we need to communicate with the RAM and the bottleneck associated with having to go through the bus to it.

The Entity Component System pattern encourages constraining data access by isolating and internally organizing data to individual components which only contain what's absolutely necessary for its functionality. In doing so, we are able to reduce how much memory we need to fetch and more easily develop systems to safely process the data in parallel while avoiding data races.

### Profiler
Once I was getting pretty happy with my final MonoBehaviour OOP implementation of the project, I took a peek at what the main computational overhead for the CPU was. What I discovered was that most of the time was spent computing the movement direction for the enemies, but also accessing my GameManager singleton and the player through it. After discovering this, I decided to cache the local player instead, as to reduce the amount of times I'd have to access the singleton and player, and this brought the program down from ~14.5 ms to ~13 ms when there were 2^11-1 active enemies.

Although I wasn't entirely happy with not being able to avoid having to normalize the movement vector, so I began looking into the Burst Compiler as a way to process this in parallel instead. Eventually I began rewriting the entire project using Unity's ECS as well, as it goes hand in hand with the Burst Compiler. After finally having converted the project over to utilize DOTS, I tried comparing the previous version for performance improvements, but noticed that with 2^11-1 active enemies, it now ran several magnitudes slower, and the profiler was informing me that a major part of this was due to the quaternion construction I was doing. This was quickly resolved however, as I realized I hadn't restarted the editor in a while, and now I was able to run the game with 2^11-1 active enemies with ~8 ms on the CPU.
