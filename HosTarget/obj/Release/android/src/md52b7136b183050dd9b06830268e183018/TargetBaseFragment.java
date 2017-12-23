package md52b7136b183050dd9b06830268e183018;


public class TargetBaseFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("HosTarget.Fragments.TargetBaseFragment, HosTarget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TargetBaseFragment.class, __md_methods);
	}


	public TargetBaseFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TargetBaseFragment.class)
			mono.android.TypeManager.Activate ("HosTarget.Fragments.TargetBaseFragment, HosTarget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
