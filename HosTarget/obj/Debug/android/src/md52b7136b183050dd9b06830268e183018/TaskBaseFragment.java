package md52b7136b183050dd9b06830268e183018;


public class TaskBaseFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("HosTarget.Fragments.TaskBaseFragment, HosTarget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TaskBaseFragment.class, __md_methods);
	}


	public TaskBaseFragment ()
	{
		super ();
		if (getClass () == TaskBaseFragment.class)
			mono.android.TypeManager.Activate ("HosTarget.Fragments.TaskBaseFragment, HosTarget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public TaskBaseFragment (int p0)
	{
		super ();
		if (getClass () == TaskBaseFragment.class)
			mono.android.TypeManager.Activate ("HosTarget.Fragments.TaskBaseFragment, HosTarget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
