#This is thread manager

These classes allow to operate a large number of threads which operate in a certain order 
(some threads should work in parallel and some threads sequentially)


I have 3 main classes:

* **ThreadManager** - this is main manager which can start and stop stages
* **Stage** - this is one stage in you scheme, can store other stages and threads
* **OneThread** - simple one thread manager

All stages start sequentially

All Threads in one stage start parralel

For example:
```cs
new Stage[]{
  new Stage("SUB1",new OneThread[]{
    new OneThread("D",(OneThread thr)=>f3(ref thr))
  }),
  new Stage("SUB2",new OneThread[]{
	new OneThread("E",(OneThread thr)=>f4(ref thr)),
	new OneThread("F",(OneThread thr)=>f5(ref thr)),
	new OneThread("G",(OneThread thr)=>f6(ref thr))
  }),
  new Stage("SUB3",new OneThread[]{
		new OneThread("H",(OneThread thr)=>f7(ref thr))
  })
}
```

We had array of 3 stages
SUB1, SUB2 and SUB3
wrich will work in order:

**SUB1(thread D) -> SUB2(threads(E,F,G) -> SUB3(thread H)**

All thread start parralel
