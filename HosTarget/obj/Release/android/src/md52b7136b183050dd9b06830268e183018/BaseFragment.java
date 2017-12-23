package md52b7136b183050dd9b06830268e183018;


public class BaseFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("HosTarget.Fragments.BaseFragment, HosTarget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BaseFragment.class, __md_methods);
	}


	public BaseFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BaseFragment.class)
			mono.android.TypeManager.Activate ("HosTarget.Fragments.BaseFragment, HosTarget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
